using System;
using System.Collections.Generic;
using UnityEngine;

// 泛型对象池类
/*
 public class ObjectPool<T> where T : class, new()
这行代码定义了一个名为 ObjectPool 的泛型类，以下是对其各部分的详细解释：
public class ObjectPool<T>
public 是访问修饰符，表示该类可以被任何其他类访问。
class 关键字用于声明一个类。
ObjectPool 是类的名称，它代表一个对象池，用于管理对象的创建、复用和回收。
<T> 表示这是一个泛型类，T 是类型参数，它可以在使用该类时被具体的类型所替代。通过使用泛型，这个对象池可以适用于任何类型的对象，提高了代码的复用性。
where T : class, new()
where 是泛型约束关键字，用于对类型参数 T 进行限制。
T : class 表示类型参数 T 必须是引用类型，即 T 可以是类、接口、委托或数组类型，但不能是值类型（如 int、float、struct 等）。
T : new() 表示类型参数 T 必须有一个无参数的公共构造函数，这样在对象池需要创建新对象时，可以使用 new T() 来实例化对象。
 */
// 泛型对象池类
public class ObjectPool<T> where T : class, new()
{
    private readonly Stack<PooledObject<T>> _objectStack;
    private readonly int _maxSize;
    private readonly Action<T> _actionOnGet;
    private readonly Action<T> _actionOnRelease;

    // 构造函数，初始化对象池
    public ObjectPool(int initialSize, int maxSize, Action<T> actionOnGet = null, Action<T> actionOnRelease = null)
    {
        _maxSize = maxSize;
        _actionOnGet = actionOnGet;
        _actionOnRelease = actionOnRelease;
        _objectStack = new Stack<PooledObject<T>>(initialSize);

        // 初始化对象池，创建指定数量的对象
        for (int i = 0; i < initialSize; i++)
        {
            var pooledObj = ScriptableObject.CreateInstance<PooledObject<T>>();
            pooledObj.Initialize(new T());
            _objectStack.Push(pooledObj);
        }
    }

    // 从对象池中获取一个对象
    public PooledObject<T> Get()
    {
        PooledObject<T> pooledObj;
        if (_objectStack.Count > 0)
        {
            pooledObj = _objectStack.Pop();
        }
        else
        {
            pooledObj = ScriptableObject.CreateInstance<PooledObject<T>>();
            pooledObj.Initialize(new T());
        }

        pooledObj.PopCount++;
        _actionOnGet?.Invoke(pooledObj.Object);
        return pooledObj;
    }

    // 将对象释放回对象池
    public void Release(PooledObject<T> pooledObj)
    {
        if (pooledObj == null)
        {
            return;
        }

        _actionOnRelease?.Invoke(pooledObj.Object);

        if (_objectStack.Count < _maxSize)
        {
            _objectStack.Push(pooledObj);
        }
        else
        {
            // 如果超出最大容量，销毁该 ScriptableObject
            ScriptableObject.Destroy(pooledObj);
        }
    }
}

// 示例类，用于测试对象池
public class TestObject
{
    public int Value { get; set; }

    public void Reset()
    {
        Value = 0;
    }
}
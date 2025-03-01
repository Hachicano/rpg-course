using System;
using System.Collections.Generic;
using UnityEngine;

// ���Ͷ������
/*
 public class ObjectPool<T> where T : class, new()
���д��붨����һ����Ϊ ObjectPool �ķ����࣬�����Ƕ�������ֵ���ϸ���ͣ�
public class ObjectPool<T>
public �Ƿ������η�����ʾ������Ա��κ���������ʡ�
class �ؼ�����������һ���ࡣ
ObjectPool ��������ƣ�������һ������أ����ڹ������Ĵ��������úͻ��ա�
<T> ��ʾ����һ�������࣬T �����Ͳ�������������ʹ�ø���ʱ������������������ͨ��ʹ�÷��ͣ��������ؿ����������κ����͵Ķ�������˴���ĸ����ԡ�
where T : class, new()
where �Ƿ���Լ���ؼ��֣����ڶ����Ͳ��� T �������ơ�
T : class ��ʾ���Ͳ��� T �������������ͣ��� T �������ࡢ�ӿڡ�ί�л��������ͣ���������ֵ���ͣ��� int��float��struct �ȣ���
T : new() ��ʾ���Ͳ��� T ������һ���޲����Ĺ������캯���������ڶ������Ҫ�����¶���ʱ������ʹ�� new T() ��ʵ��������
 */
// ���Ͷ������
public class ObjectPool<T> where T : class, new()
{
    private readonly Stack<PooledObject<T>> _objectStack;
    private readonly int _maxSize;
    private readonly Action<T> _actionOnGet;
    private readonly Action<T> _actionOnRelease;

    // ���캯������ʼ�������
    public ObjectPool(int initialSize, int maxSize, Action<T> actionOnGet = null, Action<T> actionOnRelease = null)
    {
        _maxSize = maxSize;
        _actionOnGet = actionOnGet;
        _actionOnRelease = actionOnRelease;
        _objectStack = new Stack<PooledObject<T>>(initialSize);

        // ��ʼ������أ�����ָ�������Ķ���
        for (int i = 0; i < initialSize; i++)
        {
            var pooledObj = ScriptableObject.CreateInstance<PooledObject<T>>();
            pooledObj.Initialize(new T());
            _objectStack.Push(pooledObj);
        }
    }

    // �Ӷ�����л�ȡһ������
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

    // �������ͷŻض����
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
            // �������������������ٸ� ScriptableObject
            ScriptableObject.Destroy(pooledObj);
        }
    }
}

// ʾ���࣬���ڲ��Զ����
public class TestObject
{
    public int Value { get; set; }

    public void Reset()
    {
        Value = 0;
    }
}
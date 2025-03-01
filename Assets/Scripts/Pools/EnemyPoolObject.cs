using UnityEngine;
using static UnityEditor.Progress;

#if UNITY_EDITOR
using UnityEditor;
#endif

// 封装对象和其 Pop 次数的类型，继承自 ScriptableObject
[CreateAssetMenu(fileName = "NewPooledObject", menuName = "Pooled Object")]
public class PooledObject<T> : ScriptableObject where T : class, new()
{
    public T Object { get; set; }
    public int PopCount { get; set; }
    public string objectId { get; set; }

    public void Initialize(T obj)
    {
        Object = obj;
        PopCount = 0;
    }

    private void OnValidate()
    {
        // This works on when you in Unity
        #if UNITY_EDITOR  
        string path = AssetDatabase.GetAssetPath(this);
        objectId = AssetDatabase.AssetPathToGUID(path);
        #endif
    }
}

/*
 public class PooledObject<T>
这行代码定义了一个名为 PooledObject 的泛型类，具体解释如下：
public：同样是访问修饰符，表明该类可以被其他任何类访问。
class：用于声明这是一个类。
PooledObject：类的名称，这个类通常用于封装存储在对象池中的对象，可能会包含一些与对象池管理相关的额外信息，比如对象的使用状态、Pop 次数等。
<T>：表示这是一个泛型类，T 是类型参数，在使用该类时需要指定具体的类型。PooledObject<T> 可以存储任意类型 T 的对象，增强了代码的灵活性和复用性。
    例如，当 T 为 int 时，PooledObject<int> 就可以用来封装一个 int 类型的对象；
    当 T 为自定义的 TestObject 类时，PooledObject<TestObject> 就可以用来封装一个 TestObject 类型的对象。
 */
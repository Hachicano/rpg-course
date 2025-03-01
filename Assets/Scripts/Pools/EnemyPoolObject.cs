using UnityEngine;
using static UnityEditor.Progress;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ��װ������� Pop ���������ͣ��̳��� ScriptableObject
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
���д��붨����һ����Ϊ PooledObject �ķ����࣬����������£�
public��ͬ���Ƿ������η�������������Ա������κ�����ʡ�
class��������������һ���ࡣ
PooledObject��������ƣ������ͨ�����ڷ�װ�洢�ڶ�����еĶ��󣬿��ܻ����һЩ�����ع�����صĶ�����Ϣ����������ʹ��״̬��Pop �����ȡ�
<T>����ʾ����һ�������࣬T �����Ͳ�������ʹ�ø���ʱ��Ҫָ����������͡�PooledObject<T> ���Դ洢�������� T �Ķ�����ǿ�˴��������Ժ͸����ԡ�
    ���磬�� T Ϊ int ʱ��PooledObject<int> �Ϳ���������װһ�� int ���͵Ķ���
    �� T Ϊ�Զ���� TestObject ��ʱ��PooledObject<TestObject> �Ϳ���������װһ�� TestObject ���͵Ķ���
 */
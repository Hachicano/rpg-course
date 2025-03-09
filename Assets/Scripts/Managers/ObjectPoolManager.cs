using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    public Dictionary<GameObject, List<GameObject>> objectPool = new Dictionary<GameObject, List<GameObject>>();
    [SerializeField] private int maxPoolSize = 15; // ������������Ϊ 15(�ݶ�)

    // ��Ҫ�ֶ���ӵ�����صĶ�������
    public GameObject[] initialObjects;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        // ����ʼ������ӵ������
        AddInitialObjectsToPool();
    }

    // ����ʼ������ӵ�����صķ���
    private void AddInitialObjectsToPool()
    {
        if (initialObjects != null)
        {
            foreach (GameObject obj in initialObjects)
            {
                if (obj != null)
                {
                    // �����������һ���������Ի�ȡ�����Ӧ��Ԥ����
                    // ʵ��ʹ���У��������Ҫ���ݾ�������޸Ļ�ȡԤ������߼�
                    GameObject prefab = obj.GetComponent<Enemy>().EnemySelfPrefab;
                    if (prefab != null)
                    {
                        AddToPool(prefab, obj, true);
                    }
                }
            }
        }
    }

    public GameObject getPooledObject(GameObject _prefab, Vector3 _position, Quaternion _rotation)
    {
        if (!objectPool.ContainsKey(_prefab))
        {
            objectPool[_prefab] = new List<GameObject>();
        }

        List<GameObject> pool = objectPool[_prefab];

        // ���� popCount ��С��Ϊ 0 �Ķ���
        GameObject objToReuse = null;
        int minPopCount = int.MaxValue;
        foreach (GameObject obj in pool)
        {
            ObjectSpawner spawner = obj.GetComponent<ObjectSpawner>();
            if (spawner.popCount < minPopCount)
            {
                minPopCount = spawner.popCount;
                objToReuse = obj;
            }
        }

        if (objToReuse != null && !objToReuse.activeInHierarchy)
        {
            Debug.Log("�КG");
            // ���ö��󲢳�ʼ��
            objToReuse.SetActive(true);
            objToReuse.transform.position = _position;
            objToReuse.transform.rotation = _rotation;
            objToReuse.GetComponent<ObjectSpawner>().initialize();//Ԥ����ǵù���һ������ű�
            Debug.Log("���������" + objToReuse.gameObject.name);
            return objToReuse;
        }

        if (pool.Count >= maxPoolSize)
        {
            // ���������ɾ�� popCount ��С�Ķ���
            pool.Remove(objToReuse);
            Destroy(objToReuse);
            Debug.Log("ɾ�������: " + objToReuse.gameObject.name);
        }

        // �����¶���
        GameObject newObj = Instantiate(_prefab, _position, _rotation);
        Debug.Log("���������" + newObj.gameObject.name);
        pool.Add(newObj);
        return newObj;
    }

    // �������������
    public void AddToPool(GameObject prefab, GameObject obj, bool isActive)
    {
        if (!objectPool.ContainsKey(prefab))
        {
            objectPool[prefab] = new List<GameObject>();
        }

        List<GameObject> pool = objectPool[prefab];

        if (pool.Count >= maxPoolSize)
        {
            // ���������ɾ�� popCount ��С�Ķ���
            GameObject objToRemove = null;
            int minPopCount = int.MaxValue;
            foreach (GameObject pooledObj in pool)
            {
                ObjectSpawner spawner = pooledObj.GetComponent<ObjectSpawner>();
                if (spawner.popCount < minPopCount)
                {
                    minPopCount = spawner.popCount;
                    objToRemove = pooledObj;
                }
            }

            if (objToRemove != null)
            {
                pool.Remove(objToRemove);
                Destroy(objToRemove);
            }
        }

        // ��Ӷ��󵽳�
        obj.SetActive(isActive);
        pool.Add(obj);
    }

    public void returnToPool(GameObject obj)
    {
        ObjectSpawner spawner = obj.GetComponent<ObjectSpawner>();
        if (spawner != null)
        {
            obj.SetActive(false);
        }
    }
}

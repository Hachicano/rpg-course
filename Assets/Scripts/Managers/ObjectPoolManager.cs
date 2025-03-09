using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    public Dictionary<GameObject, List<GameObject>> objectPool = new Dictionary<GameObject, List<GameObject>>();
    [SerializeField] private int maxPoolSize = 15; // 对象池最大容量为 15(暂定)

    // 需要手动添加到对象池的对象数组
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

        // 将初始对象添加到对象池
        AddInitialObjectsToPool();
    }

    // 将初始对象添加到对象池的方法
    private void AddInitialObjectsToPool()
    {
        if (initialObjects != null)
        {
            foreach (GameObject obj in initialObjects)
            {
                if (obj != null)
                {
                    // 这里假设你有一个方法可以获取对象对应的预制体
                    // 实际使用中，你可能需要根据具体情况修改获取预制体的逻辑
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

        // 查找 popCount 最小或为 0 的对象
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
            Debug.Log("有欸");
            // 启用对象并初始化
            objToReuse.SetActive(true);
            objToReuse.transform.position = _position;
            objToReuse.transform.rotation = _rotation;
            objToReuse.GetComponent<ObjectSpawner>().initialize();//预制体记得挂载一下这个脚本
            Debug.Log("启用了这个" + objToReuse.gameObject.name);
            return objToReuse;
        }

        if (pool.Count >= maxPoolSize)
        {
            // 对象池满，删除 popCount 最小的对象
            pool.Remove(objToReuse);
            Destroy(objToReuse);
            Debug.Log("删除了这个: " + objToReuse.gameObject.name);
        }

        // 创建新对象
        GameObject newObj = Instantiate(_prefab, _position, _rotation);
        Debug.Log("创建了这个" + newObj.gameObject.name);
        pool.Add(newObj);
        return newObj;
    }

    // 将对象加入对象池
    public void AddToPool(GameObject prefab, GameObject obj, bool isActive)
    {
        if (!objectPool.ContainsKey(prefab))
        {
            objectPool[prefab] = new List<GameObject>();
        }

        List<GameObject> pool = objectPool[prefab];

        if (pool.Count >= maxPoolSize)
        {
            // 对象池满，删除 popCount 最小的对象
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

        // 添加对象到池
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

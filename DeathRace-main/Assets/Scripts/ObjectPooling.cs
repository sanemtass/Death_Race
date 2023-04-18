using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private static ObjectPooling instance = null;
    public static ObjectPooling Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("ObjectPooling").AddComponent<ObjectPooling>();
            }
            return instance;
        }
    }
    private void OnEnable()
    {
        instance = this;
    }

    [Serializable]
    public struct Pool
    {
        public Queue<GameObject> PooledObjects;
        public GameObject objectPrefab;
        public int poolSize;
    }

    [SerializeField] public Pool[] pools = null;

    private void Awake()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].PooledObjects = new Queue<GameObject>();
            var PoolParent = new GameObject();
            PoolParent.transform.parent = transform;
            PoolParent.name = pools[i].objectPrefab.ToString();
            pools[i].PooledObjects = new Queue<GameObject>();
            for (int j = 0; j < pools[i].poolSize; j++)
            {
                GameObject obj = Instantiate(pools[i].objectPrefab);
                obj.SetActive(false);

                pools[i].PooledObjects.Enqueue(obj);
                obj.transform.parent = PoolParent.transform;
            }
        }
    }

    public GameObject GetPoolObject(int objectType)
    {
        if (objectType >= pools.Length) return null;
        if (pools[objectType].PooledObjects.Count == 0)
        {
            Debug.Log("Stack limit reached");
            AddSizePool(5f, objectType);
        }


        GameObject obj = pools[objectType].PooledObjects.Dequeue();
        obj.SetActive(true);
        if (objectType == 0)
        {
            pools[objectType].PooledObjects.Enqueue(obj);
        }
        return obj;
    }

    public void SetPoolObject(GameObject pooledObject, int objectType)
    {
        if (objectType >= pools.Length) return;
        pools[objectType].PooledObjects.Enqueue(pooledObject);
        pooledObject.SetActive(false);
    }

    public void AddSizePool(float amount, int objectType)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(pools[objectType].objectPrefab);
            obj.SetActive(false);
            pools[objectType].PooledObjects.Enqueue(obj);
        }
    }
}

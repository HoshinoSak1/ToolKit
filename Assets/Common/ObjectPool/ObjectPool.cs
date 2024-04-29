using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;

    private Queue<GameObject> objectPool = new Queue<GameObject>();

    public void Init(int count)
    {
        // 创建对象池中的对象
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    public GameObject GetObjectFromPool(Vector3 transform,Quaternion rotation)
    {
        // 如果对象池为空，则创建新对象并返回
        if (objectPool.Count == 0)
        {
            GameObject newObj = Instantiate(prefab, transform, rotation);
            return newObj;
        }

        // 从对象池中取出一个对象并返回
        GameObject obj = objectPool.Dequeue();
        obj.transform.position = transform;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        // 将对象重新放入对象池中
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}

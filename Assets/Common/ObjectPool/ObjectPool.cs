using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;

    private Queue<GameObject> objectPool = new Queue<GameObject>();

    public void Init(int count)
    {
        // ����������еĶ���
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    public GameObject GetObjectFromPool(Vector3 transform,Quaternion rotation)
    {
        // ��������Ϊ�գ��򴴽��¶��󲢷���
        if (objectPool.Count == 0)
        {
            GameObject newObj = Instantiate(prefab, transform, rotation);
            return newObj;
        }

        // �Ӷ������ȡ��һ�����󲢷���
        GameObject obj = objectPool.Dequeue();
        obj.transform.position = transform;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        // ���������·���������
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}
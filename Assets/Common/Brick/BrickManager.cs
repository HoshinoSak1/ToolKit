using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BrickManager
{
    public static Dictionary<int,GameObject> brickPrefabDir = new Dictionary<int,GameObject>();

    public static string path = PathUtils.GetStanderdPath("Assets\\Things\\Prefabs\\Bricks");
    public static void Init()
    {
        brickPrefabDir.Clear();
        string[] prefabPaths = Directory.GetFiles(path, "*.prefab");

        int count = 0;
        foreach (string prefabPath in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                BrickBase brickBase = prefab.GetComponent<BrickBase>();
                if (brickBase && brickBase.brickData)
                {
                    brickPrefabDir.Add(brickBase.brickData.id, prefab);
                    count ++;   
                }
                else
                {
                    Debug.LogError("����δ���� BrickBase �� Brick���������ã�At path:" + prefabPath);
                }
            }
        }
        Debug.Log(string.Format("����ȡ {0} �� Prefab ����", count));
        AssetDatabase.Refresh();
    }

}

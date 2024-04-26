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
                    Debug.LogError("存在未设置 BrickBase 的 Brick，请检查配置！At path:" + prefabPath);
                }
            }
        }
        Debug.Log(string.Format("共获取 {0} 份 Prefab 配置", count));
        AssetDatabase.Refresh();
    }

}

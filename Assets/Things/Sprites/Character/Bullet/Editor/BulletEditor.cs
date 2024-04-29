using cfg;
using cfg.Bullet;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BulletEditor : Editor
{
    public static string ConfigOutPutPath = "Assets/Configs/BulletConfigs/";

    [MenuItem("CustomTools/Create Bullet")]
    public static void CreateBullet()
    {
        string path;
        BulletDefine bulletDefine = TableDataManager.Instance.tables.BulletDefine;
       
        for(int i = 0; i < bulletDefine.DataList.Count; i++)
        {
            BulletConfigs config = bulletDefine.DataList[i];
            BulletData bulletData = CreateNewBulletData(config,out path);
            if (ConfigOutPutPath != null && !Directory.Exists(ConfigOutPutPath))
            {
                Directory.CreateDirectory(ConfigOutPutPath);
            }
            AssetDatabase.CreateAsset(bulletData, PathUtils.GetStanderdPath(ConfigOutPutPath + path));

        }

        AssetDatabase.Refresh();

    }
    public static BulletData CreateNewBulletData(BulletConfigs config, out string path)
    {
        BulletData bulletdata = ScriptableObject.CreateInstance<BulletData>();

        bulletdata.id = config.Id;
        bulletdata.name = config.Name;
        bulletdata.speed = config.Speed;
        bulletdata.livingTime = config.LivingTime;
        bulletdata.demage = config.Demage;

        //prefab通过name找prefab
        //pathutils需要预留一个根据name查找prefab的接口

        path = bulletdata.id + "Config.asset";
        return bulletdata;
    }
}

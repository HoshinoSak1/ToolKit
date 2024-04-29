using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireDefine
{
    public int bulletId;
    public int upLimit;
    public int downLimit;
    public int bulletCnt;

    public BulletData bulletData;

    public FireDefine(int bulletId, int upLimit, int downLimit, int bulletCnt)
    {
        this.bulletId = bulletId;
        this.upLimit = upLimit;
        this.downLimit = downLimit;
        this.bulletCnt = bulletCnt;

        bulletData = AssetDatabase.LoadAssetAtPath<BulletData>(PathUtils.GetBulletConfigsFromId(bulletId));
    }
}

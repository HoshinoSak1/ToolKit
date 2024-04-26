using cfg;
using cfg.Brick;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class BrickData: ScriptableObject
{
    [ReadOnly]
    public int id = default(int);
    public string prefabName = default(string);

    [HideInTables]
    public string spritePath = default(string);

    [PreviewField(Height = 40)]
    [TableColumnWidth(45, Resizable = false)]
    [ReadOnly]
    public Sprite sp;

    [HideInTables]
    public bool isCollider;

    [HideInTables]
    public bool isTrigger;

    [HideInTables]
    public bool isObstacle;
    [VerticalGroup("³¤¿í"), LabelWidth(60)]
    public int width, height;

    [HideInTables]
    [EnumPaging]
    public BrickType type;

    public BrickData Copy()
    {
        BrickData data = new BrickData();
        data.id = id;
        data.prefabName = prefabName;
        data.spritePath = spritePath;
        data.sp = sp;
        data.isCollider = isCollider;
        data.isTrigger = isTrigger;
        data.isObstacle = isObstacle;
        data.width = width;
        data.height = height;
        data.type = type;
        return data;
    }
    public void SetFromData(BrickData data)
    {
        id =  data.id;
        prefabName = data.prefabName;
        spritePath = data.spritePath;
        sp = data.sp;
        isCollider = data.isCollider;
        isTrigger = data.isTrigger;
        isObstacle = data.isObstacle;
        width = data.width;
        height = data.height;
        type = data.type;
    }
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ShowInInspector]
public class BulletData : ScriptableObject
{
    [ReadOnly]
    public int id;

    [ReadOnly]
    public string prefabName;

    public float speed;

    public float livingTime;

    public int demage;

    public GameObject prefab;
}

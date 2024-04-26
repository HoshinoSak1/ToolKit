using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData", order = 1)]
public class MapData : ScriptableObject
{
    [Title("基础信息")]
    [Required("设置ID")]
    public int mapId;
    [Required("名称")]
    public string mapName;
    [VerticalGroup("长宽"), LabelWidth(60)]
    [Required("设置长宽")]
    public int width, height;

    [Title("预览设置")]
    [TableMatrix(DrawElementMethod = "DrawCell")]
    public int[,] mapData = new int[1,1];

    public Texture2D[,] SquareCelledMatrix;
    [TableMatrix(HorizontalTitle = "Square Celled Matrix", SquareCells = true)]
    public Sprite[,] mapDataView = new Sprite[1, 1];

    [Button("初始化Map Data")]
    private void InitData()
    {
        if (width == 0 || height == 0)
        {
            Debug.LogError("请设置 宽/高");
            return;
        }
        mapData = new int[width, height];
        mapDataView = new Sprite[width, height];
    }

}

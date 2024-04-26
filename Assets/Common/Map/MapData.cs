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
    [Title("������Ϣ")]
    [Required("����ID")]
    public int mapId;
    [Required("����")]
    public string mapName;
    [VerticalGroup("����"), LabelWidth(60)]
    [Required("���ó���")]
    public int width, height;

    [Title("Ԥ������")]
    [TableMatrix(DrawElementMethod = "DrawCell")]
    public int[,] mapData = new int[1,1];

    public Texture2D[,] SquareCelledMatrix;
    [TableMatrix(HorizontalTitle = "Square Celled Matrix", SquareCells = true)]
    public Sprite[,] mapDataView = new Sprite[1, 1];

    [Button("��ʼ��Map Data")]
    private void InitData()
    {
        if (width == 0 || height == 0)
        {
            Debug.LogError("������ ��/��");
            return;
        }
        mapData = new int[width, height];
        mapDataView = new Sprite[width, height];
    }

}

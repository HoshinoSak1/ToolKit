using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    [OnValueChanged("OnValueChanged")]
    [LabelWidth(200)]
    [ShowInInspector]
    [TableMatrix(SquareCells = true, DrawElementMethod = "DrawColoredEnumElement")]
    [Required("设置长宽！")]
    [PropertyOrder(-1)]
    public int[,] mapData;

    [LabelWidth(200)]
    [ShowInInspector]
    [TableMatrix(SquareCells =true,HideColumnIndices = true,HideRowIndices = true)]
    [FoldoutGroup("Sprite预览")]
    [ReadOnly]
    [Required("设置长宽 并 初始化Map Data")]
    public Sprite[,] mapDataView;

    [Button("1.初始化Map Data")]
    [PropertyOrder(-1)]
    private void InitData()
    {
        
        if (width == 0 || height == 0)
        {
            Debug.LogError("请设置 宽/高");
            return;
        }
        mapData = new int[width, height];
        mapDataView = new Sprite[width, height];
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                mapDataView[i, j] = null;
                mapData[i, j] = -1;
            }
        }
    }

    [Button("2.初始化砖块列表")]
    [PropertyOrder(-1)]
    public void InitBrick()
    {
        //初始化所有砖块数据
        BrickManager.Init();
    }

    [ValueDropdown("GetBrick")]
    [LabelText("砖块")]
    [ShowInInspector]
    [PropertyOrder(-1)]
    public KeyValuePair<int, string> nowSelectedBrick;

    [HorizontalGroup("Brick"), LabelWidth(40)]
    [LabelText("Data")]
    [ReadOnly]
    [PropertyOrder(-1)]
    public BrickData brickdata;

    [HorizontalGroup("Brick"), LabelWidth(20)]
    [PreviewField]
    [ReadOnly]
    [PropertyOrder(-1)]
    public Sprite sp;

    private IEnumerable GetBrick()
    {
        foreach(var id in BrickManager.brickPrefabDir)
        {
            yield return new KeyValuePair<int,string>(id.Key, BrickManager.brickPrefabDir[id.Key].GetComponent<BrickBase>().brickData.name);
        }
    }

    [OnInspectorGUI]
    private void Update()
    {
        if(nowSelectedBrick.Key != 0)
        {
            brickdata = BrickManager.brickPrefabDir[nowSelectedBrick.Key].GetComponent<BrickBase>().brickData;
            sp = brickdata.sp;
        }

    }

    private void OnValueChanged()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //-1为未设置砖块
                if (mapData[i, j] == -1) continue;

                //取配置里对应ID的prefab
                GameObject brickPrefab = BrickManager.brickPrefabDir[mapData[i, j]];
                if (brickPrefab == null) return;

                //取砖块的基础信息
                BrickBase brickInfo = brickPrefab.GetComponent<BrickBase>();
                BrickData brickData = brickInfo.brickData;

                Sprite sp = brickData.sp;

                //设置直观显示
                mapDataView[i, j] = sp;

            }

        }
    }

    private int DrawColoredEnumElement(Rect rect, int value)
    {
        Texture2D spriteTexture = new Texture2D((int)rect.width, (int)rect.height) ;
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            if (brickdata == null) return -1;

            value = nowSelectedBrick.Key;
            spriteTexture = sp.texture;
            GUI.changed = true;
            Event.current.Use();
        }
        GUI.DrawTextureWithTexCoords(rect.Padding(1), 
                value == -1? new Texture2D((int)rect.width, (int)rect.height): GetTexture2D(BrickManager.brickPrefabDir[value].GetComponent<BrickBase>().brickData.sp), 
                CalculateTexCoords(rect.Padding(1), spriteTexture.width, spriteTexture.height), true);
        return value;
    }
    Rect CalculateTexCoords(Rect rect, float textureWidth, float textureHeight)
    {
        float x = rect.x / textureWidth;
        float y = rect.y / textureHeight;
        float width = rect.width / textureWidth;
        float height = rect.height / textureHeight;
        return new Rect(rect.position,new Vector2(width, height));
    }

    Texture2D GetTexture2D(Sprite sprite)
    {
        var targetTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        var pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height);
        targetTex.SetPixels(pixels);
        targetTex.Apply();
        return targetTex;
    }

    [Button("3.创建可视化展示地图")]
    private void CreateMap()
    {
        //设置左上角为地图根节点
        GameObject root = new GameObject("MapRoot");
        root.transform.position = Vector3.zero;

        for (int i = 0;i < width;i++)
        {
            for (int j = 0; j < height; j++)
            {
                //-1为未设置砖块
                if (mapData[i, j] == -1) continue;

                //取配置里对应ID的prefab
                GameObject brickPrefab = BrickManager.brickPrefabDir[mapData[i,j]];

                //取砖块的基础信息
                BrickBase brickInfo = brickPrefab.GetComponent<BrickBase>();
                BrickData brickData = brickInfo.brickData;

                int col = brickData.width;
                int row = brickData.height;
                Sprite sp = brickData.sp;

                //设置直观显示
                mapDataView[i,j] = sp;

                GameObject brickInstance = Instantiate(brickPrefab, root.transform);

                float x = root.transform.position.x + i + 0.5f * (col - 1);
                float y = root.transform.position.y + j + 0.5f * (row - 1);


                brickInstance.transform.position = new Vector3(x, -y, 0);
            }

        }
    }

}

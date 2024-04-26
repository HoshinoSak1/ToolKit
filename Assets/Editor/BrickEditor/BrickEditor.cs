using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using cfg;
using System.Runtime.CompilerServices;
using System.IO;
using static UnityEngine.GraphicsBuffer;
using cfg.Brick;
using SimpleJSON;
using static UnityEngine.UI.CanvasScaler;

public class BrickEditor: OdinEditorWindow
{
    [MenuItem("CustomTools/Brick Prefab Editor")]
    private static void OpenWindow()
    {
        GetWindow<BrickEditor>().Show();
    }

    #region 基础配置
    [Title("Brick配置")]
    [Button("获取当前已创建Prefab至Dir")]
    [PropertyOrder(-1)]
    public void Read()
    {
        BrickManager.Init();
    }

    [Title("设置路径")]
    [FolderPath]
    [LabelText("预制体保存路径")]
    [ReadOnly]
    [PropertyOrder(-1)]
    public string PrefabOutPutPath = PathUtils.GetStanderdPath("Assets\\Things\\Prefabs\\Bricks");

    [FolderPath]
    [LabelText("砖块数据保存路径")]
    [ReadOnly]
    [PropertyOrder(-1)]
    public string ConfigOutPutPath;

    #endregion

    #region 单砖块配置
    [Title("基础属性")]
    [TabGroup("生成单个Brick")]
    [LabelText("砖块ID")]
    public int id;

    [TabGroup("生成单个Brick")]
    [LabelText("预制体名称")]
    public string prefabName;

    [TabGroup("生成单个Brick")]
    [Sirenix.OdinInspector.FilePath]
    [Required("必须向图片路径拖入精灵图 或 输入对应图片路径")]
    [LabelText("图片路径")]
    public string spritePath;

    [TabGroup("生成单个Brick")]
    [LabelText("图片预览")]
    [PreviewField]
    [ReadOnly]
    public Sprite sp; 

    [TabGroup("生成单个Brick")]
    public int width = 1;
    [TabGroup("生成单个Brick")]
    public int height = 1;

    [TabGroup("生成单个Brick")]
    [EnumPaging]
    [LabelText("砖块类型")]
    public BrickType type;

    [Title("物理属性")]
    [TabGroup("生成单个Brick")]
    [LabelText("是否具有碰撞体")]
    public bool isCollider;

    [TabGroup("生成单个Brick")]
    [LabelText("是否为寻路阻碍")]
    public bool isTrigger;

    [TabGroup("生成单个Brick")]
    [LabelText("是否为寻路阻碍")]
    public bool isObstacle;

    [TabGroup("生成单个Brick")]
    [Button("生成Prefab")]
    private void CreateBrickPrefab()
    {
        string path;
        BrickData brickData = CreateNewBrickData(this.id,this.prefabName,this.spritePath,this.isCollider,this.isTrigger,
                                                this.isObstacle,this.width,this.height,this.type,out path);
        if (ConfigOutPutPath != null && !Directory.Exists(ConfigOutPutPath))
        {
            Directory.CreateDirectory(ConfigOutPutPath);
        }
        AssetDatabase.CreateAsset(brickData, PathUtils.GetStanderdPath(ConfigOutPutPath + path));

        CreatePrefab(brickData);

        AssetDatabase.Refresh();
        Debug.Log(string.Format("创建 [{0}] DataConfig 完成！ ", prefabName));
    }
    #endregion

    #region 批量砖块配置
    [TabGroup("读表生成")]
    [TableList]
    public List<BrickData> BrickDataList;

    [TabGroup("读表生成")]
    [Button("读取配置表")]
    [PropertyOrder(-1)]
    private void ReadConfig()
    {
        BrickDataList.Clear();
        string path;
        Tables tab = new Tables(LoadTable);
        foreach (var data in tab.TbBrick.DataList)
        {
            BrickData brickData = CreateNewBrickData(data.Id, data.PrefabName, data.SpritePath, data.IsCollider, data.IsTrigger,
                                                data.IsObstacle, data.Width, data.Height, data.BrickType, out path);

            if (ConfigOutPutPath != null && !Directory.Exists(ConfigOutPutPath))
            {
                Directory.CreateDirectory(ConfigOutPutPath);
            }
            AssetDatabase.CreateAsset(brickData, PathUtils.GetStanderdPath(ConfigOutPutPath + path));

            Debug.Log(string.Format("创建 [{0}] DataConfig 完成！ ", data.PrefabName));
            BrickDataList.Add(brickData);
        }
        Debug.Log(string.Format("配置表读取完成 共 [{0}] DataConfig ！ ", BrickDataList.Count));

        AssetDatabase.Refresh();
    }
    [TabGroup("读表生成")]
    [Button("生成所有Prefab")]
    [PropertyOrder(-1)]
    private void CreatePrefabByConfig()
    {
        foreach (var data in BrickDataList)
        {
           CreatePrefab(data);
        }
        Debug.Log("创建完成！");
        Debug.Log(string.Format("共创建 [{0}] 个 Prefab！", BrickManager.brickPrefabDir.Count));
    }
    #endregion

    #region 修改对应Brick
    [TabGroup("修改单项Brick")]
    public BrickBase Brick;
    [TabGroup("修改单项Brick")]
    [InlineEditor(InlineEditorModes.FullEditor)]
    public BrickData Data;
    [TabGroup("修改单项Brick")]
    [Button("应用修改")]
    public void ApplyData()
    {
        Brick.brickData.SetFromData(Data);

        Brick =  CreatePrefab(Brick.brickData).GetComponent<BrickBase>();
    }

    #endregion

    #region 特殊生命周期
    //editor的update
    [OnInspectorGUI]
    public void OnInspectorGUI()
    {
        //更新配置输出显示
        if (PrefabOutPutPath != null)
        {
            ConfigOutPutPath = PrefabOutPutPath + "/" + "Config";
        }
        //刷新sp显示
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        if (sprite != null)
        {
            sp = sprite;
        }
        if(Brick!=null)
        {
            if(Data.id != Brick.brickData.id)
                Data = Brick.brickData.Copy();
            Sprite tmpsprite = AssetDatabase.LoadAssetAtPath<Sprite>(Data.spritePath);
            if (tmpsprite != null)
            {
                Data.sp = tmpsprite;
            }
        }
    }
    #endregion

    #region 工具方法
    /// <summary>
    /// 创建Brick配置数据
    /// </summary>
    /// <param name="path">生成文件所在位置</param>
    /// <returns></returns>
    BrickData CreateNewBrickData(   int id, 
                                    string prefabName,
                                    string spritePath,
                                    bool isCollider,
                                    bool isTrigger,
                                    bool isObstacle,
                                    int width, 
                                    int height,
                                    BrickType type,
                                    out string path)
    {
        BrickData brickData = ScriptableObject.CreateInstance<BrickData>();
        brickData.id = id;
        brickData.prefabName = prefabName;
        brickData.type = type;
        brickData.spritePath = spritePath;
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        brickData.sp = sprite;
        brickData.width = width;
        brickData.height = height;

        brickData.isCollider = isCollider;
        brickData.isTrigger = isTrigger;
        brickData.isObstacle = isObstacle;
        path = "\\" + prefabName + "Config.asset";
        return brickData;
    }

    /// <summary>
    /// 根据brickdata生成prefab
    /// </summary>
    /// <param name="brickData"></param>
    GameObject CreatePrefab(BrickData brickData)
    {
        GameObject prefabGO = new GameObject(brickData.prefabName);

        //信息
        BrickBase baseComponent = prefabGO.AddComponent<BrickBase>();
        baseComponent.brickData = brickData;

        //设置显示
        if (brickData.sp)
        {
            SpriteRenderer spriteRenderer = prefabGO.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = brickData.sp;
        }

        //设置碰撞体
        if (brickData.isCollider)
        {
            BoxCollider2D boxCollider2D = prefabGO.AddComponent<BoxCollider2D>();

            if (brickData.isTrigger)
            {
                boxCollider2D.isTrigger = true;
            }
        }

        //设置寻路障碍
        if (brickData.isObstacle)
        {
            prefabGO.layer = LayerMask.NameToLayer("Obstacle");
        }

        //设置大小
        prefabGO.transform.localScale = new Vector3(brickData.width, brickData.height, 1);

        //设置特殊砖块对应脚本
        //等待附加

        DeleteLastCreatePrefab(brickData.prefabName + ".prefab",brickData.id);

        GameObject prefab =  PrefabUtility.SaveAsPrefabAsset(prefabGO, PathUtils.GetStanderdPath(PrefabOutPutPath + "/" + brickData.prefabName + ".prefab"));
        
        DestroyImmediate(prefabGO);

        Debug.Log("在指定路径生成Prefab ! " + brickData.prefabName);

        BrickManager.brickPrefabDir.Add(brickData.id, prefab);
        return prefab;
    }

    /// <summary>
    /// 检查对应路径下是否有同名prefab
    /// 有的话进行删除
    /// </summary>
    /// <param name="fileName">带".prefab"后缀的文件名</param>
    void DeleteLastCreatePrefab(string fileName,int id)
    {
        if (Directory.Exists(PrefabOutPutPath))
        {
            // 获取文件夹中的所有文件
            string[] files = Directory.GetFiles(PrefabOutPutPath);

            // 遍历所有文件
            foreach (string filePath in files)
            {
                string tmp = Path.GetFileName(filePath);
                // 检查文件名是否与指定的文件名相同
                if (tmp == fileName)
                {
                    // 删除同名文件
                    File.Delete(filePath);
                    BrickManager.brickPrefabDir.Remove(id);
                }
            }
        }
    }

    /// <summary>
    /// 这里获取对应配置表
    /// 虽然我觉得应该放在一个统一的table管理器里，再看
    /// </summary>
    /// <param name="table_name"></param>
    /// <returns></returns>
    private JSONNode LoadTable(string table_name)
    {
        var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Configs/{table_name}.json");
        return JSON.Parse(textAsset.text);
    }
    #endregion

}
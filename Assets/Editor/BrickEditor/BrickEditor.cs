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

    #region ��������
    [Title("Brick����")]
    [Button("��ȡ��ǰ�Ѵ���Prefab��Dir")]
    [PropertyOrder(-1)]
    public void Read()
    {
        BrickManager.Init();
    }

    [Title("����·��")]
    [FolderPath]
    [LabelText("Ԥ���屣��·��")]
    [ReadOnly]
    [PropertyOrder(-1)]
    public string PrefabOutPutPath = PathUtils.GetStanderdPath("Assets\\Things\\Prefabs\\Bricks");

    [FolderPath]
    [LabelText("ש�����ݱ���·��")]
    [ReadOnly]
    [PropertyOrder(-1)]
    public string ConfigOutPutPath;

    #endregion

    #region ��ש������
    [Title("��������")]
    [TabGroup("���ɵ���Brick")]
    [LabelText("ש��ID")]
    public int id;

    [TabGroup("���ɵ���Brick")]
    [LabelText("Ԥ��������")]
    public string prefabName;

    [TabGroup("���ɵ���Brick")]
    [Sirenix.OdinInspector.FilePath]
    [Required("������ͼƬ·�����뾫��ͼ �� �����ӦͼƬ·��")]
    [LabelText("ͼƬ·��")]
    public string spritePath;

    [TabGroup("���ɵ���Brick")]
    [LabelText("ͼƬԤ��")]
    [PreviewField]
    [ReadOnly]
    public Sprite sp; 

    [TabGroup("���ɵ���Brick")]
    public int width = 1;
    [TabGroup("���ɵ���Brick")]
    public int height = 1;

    [TabGroup("���ɵ���Brick")]
    [EnumPaging]
    [LabelText("ש������")]
    public BrickType type;

    [Title("��������")]
    [TabGroup("���ɵ���Brick")]
    [LabelText("�Ƿ������ײ��")]
    public bool isCollider;

    [TabGroup("���ɵ���Brick")]
    [LabelText("�Ƿ�ΪѰ·�谭")]
    public bool isTrigger;

    [TabGroup("���ɵ���Brick")]
    [LabelText("�Ƿ�ΪѰ·�谭")]
    public bool isObstacle;

    [TabGroup("���ɵ���Brick")]
    [Button("����Prefab")]
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
        Debug.Log(string.Format("���� [{0}] DataConfig ��ɣ� ", prefabName));
    }
    #endregion

    #region ����ש������
    [TabGroup("��������")]
    [TableList]
    public List<BrickData> BrickDataList;

    [TabGroup("��������")]
    [Button("��ȡ���ñ�")]
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

            Debug.Log(string.Format("���� [{0}] DataConfig ��ɣ� ", data.PrefabName));
            BrickDataList.Add(brickData);
        }
        Debug.Log(string.Format("���ñ��ȡ��� �� [{0}] DataConfig �� ", BrickDataList.Count));

        AssetDatabase.Refresh();
    }
    [TabGroup("��������")]
    [Button("��������Prefab")]
    [PropertyOrder(-1)]
    private void CreatePrefabByConfig()
    {
        foreach (var data in BrickDataList)
        {
           CreatePrefab(data);
        }
        Debug.Log("������ɣ�");
        Debug.Log(string.Format("������ [{0}] �� Prefab��", BrickManager.brickPrefabDir.Count));
    }
    #endregion

    #region �޸Ķ�ӦBrick
    [TabGroup("�޸ĵ���Brick")]
    public BrickBase Brick;
    [TabGroup("�޸ĵ���Brick")]
    [InlineEditor(InlineEditorModes.FullEditor)]
    public BrickData Data;
    [TabGroup("�޸ĵ���Brick")]
    [Button("Ӧ���޸�")]
    public void ApplyData()
    {
        Brick.brickData.SetFromData(Data);

        Brick =  CreatePrefab(Brick.brickData).GetComponent<BrickBase>();
    }

    #endregion

    #region ������������
    //editor��update
    [OnInspectorGUI]
    public void OnInspectorGUI()
    {
        //�������������ʾ
        if (PrefabOutPutPath != null)
        {
            ConfigOutPutPath = PrefabOutPutPath + "/" + "Config";
        }
        //ˢ��sp��ʾ
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

    #region ���߷���
    /// <summary>
    /// ����Brick��������
    /// </summary>
    /// <param name="path">�����ļ�����λ��</param>
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
    /// ����brickdata����prefab
    /// </summary>
    /// <param name="brickData"></param>
    GameObject CreatePrefab(BrickData brickData)
    {
        GameObject prefabGO = new GameObject(brickData.prefabName);

        //��Ϣ
        BrickBase baseComponent = prefabGO.AddComponent<BrickBase>();
        baseComponent.brickData = brickData;

        //������ʾ
        if (brickData.sp)
        {
            SpriteRenderer spriteRenderer = prefabGO.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = brickData.sp;
        }

        //������ײ��
        if (brickData.isCollider)
        {
            BoxCollider2D boxCollider2D = prefabGO.AddComponent<BoxCollider2D>();

            if (brickData.isTrigger)
            {
                boxCollider2D.isTrigger = true;
            }
        }

        //����Ѱ·�ϰ�
        if (brickData.isObstacle)
        {
            prefabGO.layer = LayerMask.NameToLayer("Obstacle");
        }

        //���ô�С
        prefabGO.transform.localScale = new Vector3(brickData.width, brickData.height, 1);

        //��������ש���Ӧ�ű�
        //�ȴ�����

        DeleteLastCreatePrefab(brickData.prefabName + ".prefab",brickData.id);

        GameObject prefab =  PrefabUtility.SaveAsPrefabAsset(prefabGO, PathUtils.GetStanderdPath(PrefabOutPutPath + "/" + brickData.prefabName + ".prefab"));
        
        DestroyImmediate(prefabGO);

        Debug.Log("��ָ��·������Prefab ! " + brickData.prefabName);

        BrickManager.brickPrefabDir.Add(brickData.id, prefab);
        return prefab;
    }

    /// <summary>
    /// ����Ӧ·�����Ƿ���ͬ��prefab
    /// �еĻ�����ɾ��
    /// </summary>
    /// <param name="fileName">��".prefab"��׺���ļ���</param>
    void DeleteLastCreatePrefab(string fileName,int id)
    {
        if (Directory.Exists(PrefabOutPutPath))
        {
            // ��ȡ�ļ����е������ļ�
            string[] files = Directory.GetFiles(PrefabOutPutPath);

            // ���������ļ�
            foreach (string filePath in files)
            {
                string tmp = Path.GetFileName(filePath);
                // ����ļ����Ƿ���ָ�����ļ�����ͬ
                if (tmp == fileName)
                {
                    // ɾ��ͬ���ļ�
                    File.Delete(filePath);
                    BrickManager.brickPrefabDir.Remove(id);
                }
            }
        }
    }

    /// <summary>
    /// �����ȡ��Ӧ���ñ�
    /// ��Ȼ�Ҿ���Ӧ�÷���һ��ͳһ��table��������ٿ�
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
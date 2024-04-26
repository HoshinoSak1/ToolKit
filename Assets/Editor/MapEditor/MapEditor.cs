using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Codice.CM.Client.Differences.Graphic;
using Sirenix.Utilities.Editor;
using UnityEditor.VersionControl;

public class MapEditor : OdinMenuEditorWindow
{
    static Color illegalColor = Color.red;
    static Color validColor = Color.green;

    [MenuItem("CustomTools/Map Editor")]
    private static void OpenWindow()
    {
        GetWindow<MapEditor>().Show();
    }

    private CreateNewMap createNewMap;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (createNewMap != null)
            DestroyImmediate(createNewMap.mapData);
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        createNewMap = new CreateNewMap();
        tree.Add("Create New", createNewMap);

        tree.AddAllAssetsAtPath("Map Data", "Assets/Configs/MapData", typeof(MapData));

        return tree;
    }
    protected override void OnBeginDrawEditors()
    {
        base.OnBeginDrawEditors();
        OdinMenuTreeSelection selected = this.MenuTree.Selection;
        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton("Delete Current"))
            {
                MapData asset = selected.SelectedValue as MapData;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();

            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }
}

public class CreateNewMap
{
    public CreateNewMap()
    {
        mapData = ScriptableObject.CreateInstance<MapData>();
        mapData.mapName = "New Map Data";
    }
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
    public MapData mapData;



    [Button("创建新的Map Data")]
    private void CreateNewData()
    {
        if(mapData.mapName == null)
        {
            Debug.LogError("请设置 地图名");
            return;
        }

        AssetDatabase.CreateAsset(mapData, "Assets/Configs/MapData/" + mapData.mapName + ".asset");
        AssetDatabase.SaveAssets();

        mapData = ScriptableObject.CreateInstance<MapData>();
        mapData.mapName = "New Map Data";
    }
}
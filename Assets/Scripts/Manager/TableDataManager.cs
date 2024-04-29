using cfg;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TableDataManager : Singleton<TableDataManager>
{
    public Tables tables;
    public TableDataManager()
    {
        tables = new Tables(LoadTable);
    }
    private JSONNode LoadTable(string table_name)
    {
        var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Configs/TableConfigs/{table_name}.json");
        return JSON.Parse(textAsset.text);
    }
}

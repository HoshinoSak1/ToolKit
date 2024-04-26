using cfg;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LubanDataTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var tables = new Tables(LoadTable);
        
    }

    private JSONNode LoadTable(string table_name)
    {
        var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Configs/{table_name}.json");
        return JSON.Parse(textAsset.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

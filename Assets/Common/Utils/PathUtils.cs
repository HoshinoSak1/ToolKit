using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 路径工具
/// </summary>
public static class PathUtils
{
    // 路径 ： Assets/
    public static readonly string AssetPath = Application.dataPath + "/";

    // Lua脚本路径
    public static readonly string LuaPath = "Assets/Scripts/LuaScripts/";

    //子弹配置路径
    public static readonly string bulletConfigPath = "Assets/Configs/BulletConfigs/";

    /// <summary>
    /// 原本文件夹路径获得Unity标准文件路径
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public static string GetStanderdPath(string path)
    {
        if(string.IsNullOrEmpty(path))return string.Empty;

        return path.Trim().Replace("\\", "/");
    }
    public static string GetRequirePath(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;

        return path.Trim().Replace("/", ".");
    }
    /// <summary>
    /// 获得Assets下的相对路径
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public static string GetUntiyPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;
        return path.Trim().Substring(path.IndexOf("Assets"));
    }

    public static string GetLuaPath(string path)
    {
        path = GetStanderdPath((string)path);
        path = GetUntiyPath((string)path);
        return path.Replace(".lua", "");
    }

    public static string GetBulletConfigsFromId(int id)
    {
        string path = bulletConfigPath + id.ToString() + "Config.asset";

        return path;
    }

    public static string GetBulletPrefabFromId(int id)
    {
        string path = bulletConfigPath + id.ToString() + "Config.asset";

        return path;
    }
    public static string GetBulletPrefabFromName(string name)
    {
        string path = "";

        return path;
    }
}

using System.Collections;
using System.Collections.Generic;
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
}

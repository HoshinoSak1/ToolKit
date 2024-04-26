using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ·������
/// </summary>
public static class PathUtils
{
    // ·�� �� Assets/
    public static readonly string AssetPath = Application.dataPath + "/";

    // Lua�ű�·��
    public static readonly string LuaPath = "Assets/Scripts/LuaScripts/";

    /// <summary>
    /// ԭ���ļ���·�����Unity��׼�ļ�·��
    /// </summary>
    /// <param name="path">·��</param>
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
    /// ���Assets�µ����·��
    /// </summary>
    /// <param name="path">·��</param>
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

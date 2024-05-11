using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// 快速导出UnityPackage包名
/// Ctrl + e 快速导出
/// </summary>
public partial class ExportUnityPackage
{
#if UNITY_EDITOR
    [MenuItem("Tool/导出 UnityPackage %e", false, 1)]
    private static void MenuClicked()
    {
        // 获取包名
        var generatePackageName = ExportUnityPackage.GenerateUnityPackageName();

        // 生成 UnityPackage 资源
        ExportPackage("Assets/FEvent", generatePackageName + ".unitypackage");

        // 打开生成 UnityPackage 资源的路径查看 
        OpenInFolder(Path.Combine(Application.dataPath, "../"));
    }
#endif

    /// <summary>
    /// 获取包名
    /// </summary>
    /// <returns>返回指定的包名</returns>
    public static string GenerateUnityPackageName()
    {
        return "FEvent";
    }

    /// <summary>
    /// 打开指定文件夹
    /// </summary>
    /// <param name="folderPath">文件夹路径</param>
    public static void OpenInFolder(string folderPath)
    {
        Application.OpenURL("file:///" + folderPath);
    }

    /// <summary>
    /// 导出资源
    /// </summary>
    /// <param name="assetPathName">资源路径</param>
    /// <param name="fileName">资源包名</param>
    public static void ExportPackage(string assetPathName, string fileName)
    {
        AssetDatabase.ExportPackage(assetPathName, fileName, ExportPackageOptions.Recurse);
    }
}

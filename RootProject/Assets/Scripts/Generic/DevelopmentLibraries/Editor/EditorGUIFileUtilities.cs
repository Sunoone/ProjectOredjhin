using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

// Source: https://answers.unity.com/questions/486545/getting-all-assets-of-the-specified-type.html
public static class EditorGUIFileUtilities {

    public static T[] FindAssetsWithExtension<T>(string fileExtension) where T : UnityEngine.Object
    {
        var paths = FindAssetPathsWithExtension(fileExtension);
        if (paths == null || paths.Length == 0)
            return null;

        List<T> assetsOfType = new List<T>();
        for (int i = 0; i < paths.Length; i++)
        {
            var asset = AssetDatabase.LoadAssetAtPath(paths[i], typeof(T)) as T;
            if (asset == null || (asset is T) == false)
                continue;

            assetsOfType.Add(asset);
        }

        return assetsOfType.ToArray();
    }


    public static string[] FindAssetPathsWithExtension(string fileExtension)
    {
        if (string.IsNullOrEmpty(fileExtension))
            return null;

        if (fileExtension[0] == '.')
            fileExtension = fileExtension.Substring(1);

        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*." + fileExtension, SearchOption.AllDirectories);

        List<string> assetPaths = new List<string>();
        foreach (var file in fileInfos)
        {
            var assetPath = file.FullName.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            assetPaths.Add(assetPath);
        }

        return assetPaths.ToArray();
    }




}

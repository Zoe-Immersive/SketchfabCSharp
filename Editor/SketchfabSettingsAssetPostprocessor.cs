using System.IO;
using UnityEditor;
using UnityEngine;

public class SketchfabSettingsAssetPostprocessor : AssetPostprocessor
{
    private const string InstallRelativePath = "Sketchfab/Resources";

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (File.Exists(Path.Combine(Application.dataPath, InstallRelativePath, "SketchfabSettings.asset")))
        {
            return;
        }

        foreach (string importedAsset in importedAssets)
        {
            if (!importedAsset.Contains("SketchfabSettings.asset"))
            {
                continue;
            }

            Install(Path.Combine(Directory.GetParent(Application.dataPath).FullName, importedAsset));
        }
    }

    static private void Install(string packageSettingsAbsoluteFilePath)
    {
        string installAbsolutePath = Path.Combine(Application.dataPath, InstallRelativePath);

        if (!Directory.Exists(installAbsolutePath))
        {
            Directory.CreateDirectory(installAbsolutePath);
        }

        File.Copy(packageSettingsAbsoluteFilePath, Path.Combine(installAbsolutePath, "SketchfabSettings.asset"));

        AssetDatabase.ImportAsset(Path.Combine("Assets", InstallRelativePath, "SketchfabSettings.asset"), ImportAssetOptions.ForceUpdate);

        EditorUtility.DisplayDialog("SketchfabSettings installed", "SketchfabSettings.asset was installed to Assets/Sketchfab/Resources", "OK");
    }
}

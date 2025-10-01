#if UNITY_EDITOR

using System.Diagnostics;
using System.IO;
using System.Linq;

class AutoLock : UnityEditor.AssetModificationProcessor
{
    // Called when assets are about to be opened for editing
    static string[] OnWillSaveAssets(string[] paths)
    {
        string[] lockExtensions = { ".unity", ".prefab", ".anim", ".controller", ".asset",
        ".fbx", ".png", ".jpg", ".wav", ".mp3" };

        foreach (var path in paths)
        {
            if (lockExtensions.Contains(Path.GetExtension(path)))
            {
                LockFile(path);
            }
        }

        return paths;
    }

    static void LockFile(string assetPath)
    {
        string fullPath = Path.GetFullPath(assetPath);

        // git lfs lock command
        var process = new Process();
        process.StartInfo.FileName = "git";
        process.StartInfo.Arguments = $"lfs lock \"{fullPath}\"";

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        string result = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (error != "")
        {
            if (error.Contains("Lock exists"))
                UnityEngine.Debug.Log($"[AutoLock] {assetPath} is already locked by you -> {result} {error}");
            else
            {
                UnityEngine.Debug.LogError($"[AutoLock] Failed to lock {assetPath} -> {result} {error}");

                if (error.Contains("already locked"))
                    UnityEngine.Debug.LogWarning($"{assetPath} is already locked by someone else! You may overwrite their work.");
            }
        }
        else
            UnityEngine.Debug.Log($"[AutoLock] Locked {assetPath} -> {result} {error}");
    }
}

#endif
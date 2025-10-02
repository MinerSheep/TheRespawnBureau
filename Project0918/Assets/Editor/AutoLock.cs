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

    public static string GetLockOwner(string repoPath, string relativePath)
    {
        using (Process proc = new Process())
        {
            proc.StartInfo.FileName = "git";
            proc.StartInfo.Arguments = $"lfs locks \"{relativePath}\"";
            proc.StartInfo.WorkingDirectory = repoPath;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;

            proc.Start();

            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            return output.Trim();
        }
    }

    static void LockFile(string assetPath)
    {
        string fullPath = Path.GetFullPath(assetPath);

        // git lfs lock command
        var process = new Process();
        process.StartInfo.FileName = "git";
        process.StartInfo.Arguments = $"ls-files --error-unmatch \"{fullPath}\"";

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            UnityEngine.Debug.Log("This is a newly created file, no need to lock it");
            return;
        }

        process = new Process();
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

        int exit = process.ExitCode;

        if (error != "")
        {
            if (error.Contains("Lock exists"))
            {
                UnityEngine.Debug.Log($"[AutoLock] {assetPath} is already locked by you -> {result} {error} Code: {exit}");

                //UnityEngine.Debug.LogError($"{assetPath} is already locked by someone else! You may overwrite their work! -> {result} {error} Code: {exit}");
            }
            else
            {
                UnityEngine.Debug.LogError($"[AutoLock] Failed to lock {assetPath} (It is probably locked by someone else!) -> {result} {error} Code: {exit}");
            }
        }
        else
            UnityEngine.Debug.Log($"[AutoLock] Locked {assetPath} -> {result} {error} {exit}");
    }
}

#endif
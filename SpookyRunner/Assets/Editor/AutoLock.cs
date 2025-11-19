#if UNITY_EDITOR

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

class AutoLock : UnityEditor.AssetModificationProcessor
{
    // Called when assets are about to be opened for editing
    static string[] OnWillSaveAssets(string[] paths)
    {
        string[] lockExtensions = { ".unity", ".prefab", ".anim", ".controller", ".asset" };

        foreach (var path in paths)
        {
            if (lockExtensions.Contains(Path.GetExtension(path)))
            {
                LockFile(path);
            }
        }

        return paths;
    }

    private static string RunGit(string arguments, out int exit)
    {
        using var proc = new Process();
        proc.StartInfo.FileName = "git";
        proc.StartInfo.Arguments = arguments;

        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.Start();
        proc.WaitForExit();

        string stdout = proc.StandardOutput.ReadToEnd();
        string stderr = proc.StandardError.ReadToEnd();
        proc.WaitForExit();

        exit = proc.ExitCode;

        if (exit != 0 && !string.IsNullOrEmpty(stderr))
            return stderr.Trim();
        return stdout.Trim();
    }

    public static string GetLockOwner(string assetPath)
    {
        using (Process proc = new Process())
        {
            proc.StartInfo.FileName = "git";
            proc.StartInfo.Arguments = $"lfs locks \"{assetPath}\"";

            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;

            proc.Start();

            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

           // Split input into lines
            string[] lines = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (line.Contains(assetPath, StringComparison.OrdinalIgnoreCase))
                {
                    // Split the line by tab and take the second entry (owner)
                    string[] parts = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        string owner = parts[1].Trim();
                        Console.WriteLine(owner);  // Output: MinerSheep
                        return owner;
                    }
                    break; // found it, exit loop
                }
            }

            // If not found, return null
            return null;
        }
    }

    static void LockFile(string assetPath)
    {
        string fullPath = Path.GetFullPath(assetPath);

        int exit = 0;

        // git lfs lock command
        RunGit($"ls-files --error-unmatch \"{fullPath}\"", out exit);

        if (exit != 0)
        {
            UnityEngine.Debug.Log("This is a newly created file, no need to lock it");
            return;
        }

        string response = RunGit($"lfs lock \"{fullPath}\"", out exit);
        int exitcode = exit;

        if (exit != 0)
        {
            string myEmail = RunGit("config user.email", out exit);
            string myName = RunGit("config user.name", out exit);
            string owner = GetLockOwner(assetPath);

            if (!string.IsNullOrEmpty(owner) &&
                (owner.Contains(myEmail, System.StringComparison.OrdinalIgnoreCase) ||
                 owner.Contains(myName, System.StringComparison.OrdinalIgnoreCase)))
                UnityEngine.Debug.Log($"[AutoLock] {assetPath} is already locked by you -> {response} Code: {exitcode}");
            else
                UnityEngine.Debug.LogError($"{assetPath} is already locked by someone else! You may overwrite their work! -> {response}, Owner: {owner}, Code: {exitcode}");
        }
        else
            UnityEngine.Debug.Log($"[AutoLock] Locked {assetPath} -> {response} Code: {exitcode}");
    }
}

#endif
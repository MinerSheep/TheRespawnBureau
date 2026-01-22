using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using System.Collections.Generic;

public class UpdateProcedure : MonoBehaviour
{
    public static UpdateProcedure instance { get; private set; }

    [Header("Required Labels")]
    [SerializeField] private List<string> requiredLabels = new() { "core" };

    public event Action<float> OnProgress;
    public event Action OnCompleted;
    public event Action<string> OnFailed;

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void BeginBootstrap()
    {
        StartCoroutine(BootstrapRoutine());
    }

    private IEnumerator BootstrapRoutine()
    {
        // Check for catalog updates
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;

        if (checkHandle.Status != AsyncOperationStatus.Succeeded)
        {
            OnFailed?.Invoke("Failed to check for catalog updates");
            yield break;
        }

        if (checkHandle.Result.Count > 0)
        {
            var updateHandle = Addressables.UpdateCatalogs(checkHandle.Result, false);
            yield return updateHandle;

            if (updateHandle.Status != AsyncOperationStatus.Succeeded)
            {
                OnFailed?.Invoke("Failed to update catalog");
                yield break;
            }
        }

        yield return DownloadRequiredContent();

        OnCompleted?.Invoke();
    }

    private IEnumerator DownloadRequiredContent()
    {
        Dictionary<string, long> sizeByLabel = new Dictionary<string, long>();

        long totalDownloadSize = 0;

        // Calculate download size
        foreach (var label in requiredLabels)
        {
            var sizeHandle = Addressables.GetDownloadSizeAsync(label);
            yield return sizeHandle;

            if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
            {
                OnFailed?.Invoke($"Failed to get download size '{label}'");
                yield break;
            }

            sizeByLabel[label] = sizeHandle.Result;
            totalDownloadSize += sizeHandle.Result;            
        }

        if (totalDownloadSize == 0)
        {
            OnProgress?.Invoke(1f);
            yield break;
        }

        long downloadedSize = 0;

        // Perform the download and update download counters
        foreach (var label in requiredLabels)
        {
            var downloadHandle = Addressables.DownloadDependenciesAsync(label, true);

            while (!downloadHandle.IsDone)
            {
                float percentCompleted = downloadHandle.PercentComplete;
                float percentTotal = (downloadedSize + (long)(percentCompleted * sizeByLabel[label]))
                                        / (float)totalDownloadSize;

                OnProgress?.Invoke(percentTotal);
                yield return null;
            }

            //if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
            //{
            //    OnFailed?.Invoke($"Failed downloading dependencies for label '{label}'");
            //    yield break;
            //}

            downloadedSize += totalDownloadSize;
            //Addressables.Release(downloadHandle);
        }

        OnProgress?.Invoke(1f);
    }
}

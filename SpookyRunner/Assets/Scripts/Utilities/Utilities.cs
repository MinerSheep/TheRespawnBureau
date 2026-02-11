using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Threading.Tasks;

namespace Game.Utilities
{
    public static class SceneLoader
    {
        public static string lastAddressKey = "AutoRunnerInfinite";

        public static async Task ReloadSceneAsync()
        {
            // OPTIONAL: Check if this scene is in the build list...
            
            AudioManager.instance.PlaySound("transition");
            await LoadSceneAsync(lastAddressKey);
        }
        public static async Task LoadSceneAsync(string addressKey)
        {
            var handle = Addressables.DownloadDependenciesAsync("default");
            while (!handle.IsDone)
            {
                float percentCompleted = handle.PercentComplete;
                Debug.Log(percentCompleted);
                await Task.Yield();
            }
            // Load scene from server
            var loadHandle = Addressables.LoadSceneAsync(addressKey);
            await loadHandle.Task;
        }
    }
}

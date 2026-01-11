using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableInstantiate : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject instantiatethis;

    void Start()
    {
        // Get the version number through a file on the site with the number in json format
        AddressablesRuntimeProperties.SetPropertyValue(
            "ContentVersion",
            "v1.0.0"
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            instantiatethis.LoadAssetAsync()
                .Completed += OnAddressableLoaded;
        }
    }

    void OnAddressableLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
            Instantiate(handle.Result);
        else
            Debug.LogError("Loading asset failed");
    }
}

using UnityEngine;

// Login screen is responsible for device id, password, and starting off the update procedure attached to manager
public class LoginScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Retrieve and display device id

        // Begin bootstrapping and assign loading bar progress to OnProgress
        UpdateProcedure.instance.BeginBootstrap();

        UpdateProcedure.instance.OnProgress += UpdateLoadingBar;
        UpdateProcedure.instance.OnCompleted += DisplayLogin;
        UpdateProcedure.instance.OnFailed += DisplayError;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateLoadingBar(float progress)
    {

    }

    void DisplayLogin()
    {

    }

    void DisplayError(string failReason)
    {

    }

    private void OnDestroy()
    {
        UpdateProcedure.instance.OnProgress -= UpdateLoadingBar;
        UpdateProcedure.instance.OnCompleted -= DisplayLogin;
        UpdateProcedure.instance.OnFailed -= DisplayError;
    }
}

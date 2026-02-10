using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Login screen is responsible for device id, password, and starting off the update procedure attached to manager
public class LoginScreen : MonoBehaviour
{
    Transform loadingProgress;
    Transform loadingText;
    Transform passwordInput;
    Transform uidText;

    private string deviceId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingProgress = transform.Find("LoadingProgress");
        loadingText = transform.Find("LoadingText");
        passwordInput = transform.Find("PasswordInput");
        uidText = transform.Find("UID");

        passwordInput.gameObject.SetActive(false);

        // Set up password detection
        TMP_InputField ifield = passwordInput.GetComponent<TMP_InputField>();
        ifield.onValueChanged.AddListener(OnPasswordChange);
        ifield.onEndEdit.AddListener(OnPasswordEntered);
        ifield.characterLimit = 10;

        // Retrieve and display device id
        deviceId = SystemInfo.deviceUniqueIdentifier;
        uidText.GetComponent<TextMeshProUGUI>().text = "UID: " + deviceId;

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
        loadingProgress.GetComponent<Image>().fillAmount = progress;
    }

    void DisplayLogin()
    {
        passwordInput.gameObject.SetActive(true);
        loadingProgress.gameObject.SetActive(false);
        loadingText.gameObject.SetActive(false);
    }

    void DisplayError(string failReason)
    {
        loadingText.GetComponent<TextMeshProUGUI>().text = "FAILED: " + failReason;

        // After 3 secs unlock
        Invoke(nameof(DisplayLogin), 3f);
    }
    
    void OnPasswordChange(string password)
    {
        string errstring = "";

        if (password.Length >= 10)
        {
            errstring += "Password cannot be longer than 10 characters.\n";
        }

        passwordInput.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = errstring;
    }

    void OnPasswordEntered(string password)
    {
        passwordInput.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Password was entered!";
    }

    private void OnDestroy()
    {
        UpdateProcedure.instance.OnProgress -= UpdateLoadingBar;
        UpdateProcedure.instance.OnCompleted -= DisplayLogin;
        UpdateProcedure.instance.OnFailed -= DisplayError;
    }
}

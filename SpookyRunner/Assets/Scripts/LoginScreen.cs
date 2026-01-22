using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Login screen is responsible for device id, password, and starting off the update procedure attached to manager
public class LoginScreen : MonoBehaviour
{
    Transform welcomeText;
    Transform loadingText;
    Transform passwordInput;

    private string deviceId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        welcomeText = transform.Find("WelcomeText");
        loadingText = transform.Find("LoadingText");
        passwordInput = transform.Find("PasswordInput");

        passwordInput.gameObject.SetActive(false);

        // Set up password detection
        TMP_InputField ifield = passwordInput.GetComponent<TMP_InputField>();
        ifield.onValueChanged.AddListener(OnPasswordChange);
        ifield.onEndEdit.AddListener(OnPasswordEntered);
        ifield.characterLimit = 10;

        // Retrieve and display device id
        deviceId = SystemInfo.deviceUniqueIdentifier;
        welcomeText.GetComponent<TextMeshProUGUI>().text = "Welcome, user " + deviceId;

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
        loadingText.Find("LoadingProgress").GetComponent<Image>().fillAmount = progress;
    }

    void DisplayLogin()
    {
        passwordInput.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(false);
    }

    void DisplayError(string failReason)
    {

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

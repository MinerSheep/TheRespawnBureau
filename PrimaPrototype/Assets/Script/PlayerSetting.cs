using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetting : MonoBehaviour
{
    public static KeyCode playerJumpKeyControl = KeyCode.W;
    public static KeyCode playerMoveLeftKeyControl = KeyCode.A;
    public static KeyCode playerMoveRightKeyControl = KeyCode.D;
    public static KeyCode playerCrouchKeyControl = KeyCode.LeftControl;
    public static KeyCode playerFlipKeyControl = KeyCode.Mouse1;
    public static KeyCode playerDownKeyControl = KeyCode.S;
    public static float playerVolumn = 0.3f;
    [SerializeField] private int playerKeyControlChoice = 1;
    [SerializeField] private Slider volumnSlider;

    public void Start()
    {
        volumnSlider.value = PlayerPrefs.GetFloat("playerVolumn");
        playerKeyControlChoice = PlayerPrefs.GetInt("controlChoice");
    }
    public void SetControlChoice(int controlChoice)
    {
        playerKeyControlChoice = controlChoice;
    }

    public void SettingPopUp()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ApplySetting()
    {

        if (playerKeyControlChoice == 1)
        {
            playerCrouchKeyControl = KeyCode.LeftControl;
            playerJumpKeyControl = KeyCode.W;
            playerMoveLeftKeyControl = KeyCode.A;
            playerMoveRightKeyControl = KeyCode.D;
            playerFlipKeyControl = KeyCode.Mouse1;
            playerDownKeyControl = KeyCode.S;
        }
        if (playerKeyControlChoice == 2)
        {
            playerCrouchKeyControl = KeyCode.LeftControl;
            playerJumpKeyControl = KeyCode.UpArrow;
            playerMoveLeftKeyControl = KeyCode.LeftArrow;
            playerMoveRightKeyControl = KeyCode.RightArrow;
            playerFlipKeyControl = KeyCode.LeftShift;
            playerDownKeyControl = KeyCode.DownArrow;
        }

        playerVolumn = volumnSlider.value;

        PlayerPrefs.SetInt("controlChoice", playerKeyControlChoice);
        PlayerPrefs.SetFloat("playerVolumn", playerVolumn);
        GlobalDataStatic.volume = PlayerPrefs.GetFloat("playerVolumn");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MobileButtonManager : MonoBehaviour
{
    public Button JumpButton;
    public Button SlideButton;
    public Button DashButton;

    void Start()
    {
        if (JumpButton == null)
            JumpButton = transform.Find("Button_Jump")?.GetComponent<Button>();
        if (SlideButton == null)
            SlideButton = transform.Find("Button_Slide")?.GetComponent<Button>();
        if (DashButton == null)
            DashButton = transform.Find("Button_Dash")?.GetComponent<Button>();
    }
    
    public void AssignButton(InputBuffer buffer, string action, bool hold)
    {
        Button b;
        switch (action)
        {
            case "Jump":
                b = JumpButton;
                break;
            case "Crouch":
                b = SlideButton;
                break;
            case "Dash":
                b = DashButton;
                break;    
            default:
                Debug.LogError("Don't know what button you want to assign - mobilehud playercontroller");
                return;
        }

        if (b == null)
        {
            Debug.LogError("No button for " + action + " - mobilehud");
            return; 
        }

        b.GetComponent<MobileButton>().holdable = hold;
        b.GetComponent<MobileButton>().onClick = null;
        b.GetComponent<MobileButton>().onRelease = null;

        if (hold)
        {
            b.GetComponent<MobileButton>().onClick += () => buffer.StartHold(action);
            b.GetComponent<MobileButton>().onRelease += () => buffer.EndHold(action);
        }

        b.GetComponent<MobileButton>().onClick += () => buffer.AddToBuffer(action);
    }
}

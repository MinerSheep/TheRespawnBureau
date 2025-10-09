using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject PlayButton;
    public GameObject OnlineButton;
    public GameObject OptionsButton;
    public GameObject QuitButton;

    public Vector3 initialScale;
    public Vector3 hoverScale;

    public void Start()
    {
        initialScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //make hovered menu button bigger
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }



}

using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Vector3 initialScale;
    public float hoverScaleMultiplier = 1.1f;

    public void Start()
    {
        initialScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = initialScale * hoverScaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = initialScale;
    }



}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 initialScale;
    public float hoverScaleMultiplier = 1.1f;
    public Color hoverColor;
    public Color unhoverColor;
    public TextMeshProUGUI text;

    public void Start()
    {
        initialScale = transform.localScale;
        text.color = unhoverColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = initialScale * hoverScaleMultiplier;
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = initialScale;
        text.color = unhoverColor;
    }

}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool holdable = false;

    public System.Action onClick;
    public System.Action onRelease;

    private bool isHolding = false;
    private float holdTime = 0f;

    void Update()
    {
        if (isHolding)
            holdTime += Time.deltaTime;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (holdable)
            onClick?.Invoke();
        
        isHolding = true;
        holdTime = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (holdable)
            onRelease?.Invoke();
        else
            onClick?.Invoke();

        isHolding = false;
        holdTime = 0f;
    }
}
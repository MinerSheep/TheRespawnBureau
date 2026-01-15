using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float minSwipeDistance = 80.0f;
    [SerializeField] float maxVerticalDeviation = 0.5f;

    //[Header("References")]


    private Vector2 swipeStart;
    private bool isSwiping;

    // Update is called once per frame
    void Update()
    {
        if (DeviceDetector.IsMobile)
        {
            HandleTouch();   
        }
    }

    void HandleTouch()
    {
        // no touching
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began) 
        {
            swipeStart = touch.position;
            isSwiping = true;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            DetectSwipe(touch.position);
            isSwiping = false;
        }
    }

    void DetectSwipe(Vector2 swipeEnd)
    {
        Vector2 delta = swipeEnd - swipeStart;

        // swipe fail
        if (delta.magnitude < minSwipeDistance)
            return;

        Vector2 direction = delta.normalized;

        if (direction.x > 0.8f && Mathf.Abs(direction.x) < maxVerticalDeviation)
        {
            // dash
            PlayerController pc = FindAnyObjectByType<PlayerController>();
            if (pc != null)
            {
                pc.GetInputBuffer().AddToBuffer("Dash");
            }
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileInputEvents
{
    public static event Action<Vector2> OnSwipe;

    public static void InvokeOnSwipe(Vector2 direction)
    {
        OnSwipe?.Invoke(direction);
    }
}

public class MobileInputManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float minSwipeDistance = 80.0f;
    [SerializeField] float maxVerticalDeviation = 0.5f;

    Transform s, f;
    public GameObject prefab;
    public Transform canvas;

    //[Header("References")]


    private Vector2 swipeStart;
    private bool isSwiping;

    void Start()
    {
        canvas = FindAnyObjectByType<Canvas>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (true /*DeviceDetector.IsMobile*/)
        {
            HandleTouch();   
        }

        if (Input.GetKeyDown(KeyCode.V))
            s = Instantiate(prefab, canvas).transform;
    }

    void HandleTouch()
    {
        // no touching
        if (Input.touchCount == 0)
            return;

        Debug.Log("touch detected!!");

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
        if (s == null) s = Instantiate(prefab, canvas).transform;
        if (f == null) f = Instantiate(prefab, canvas).transform;

        s.position = swipeStart; 
        f.position = swipeEnd;

        Vector2 delta = swipeEnd - swipeStart;

        // swipe fail
        if (delta.magnitude < minSwipeDistance)
            return;

        Vector2 direction = delta.normalized;

        MobileInputEvents.InvokeOnSwipe(direction);

        if (direction.x > 0.8f)
        {
            f.GetComponent<Image>().color = Color.red;

            // dash
            PlayerController pc = FindAnyObjectByType<PlayerController>();
            if (pc != null)
            {
                pc.GetInputBuffer().AddToBuffer("Dash");
            }
        }
        else
            f.GetComponent<Image>().color = Color.green;
    }

    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        canvas = FindAnyObjectByType<Canvas>().transform;
    }

    void OnEnable() { SceneManager.activeSceneChanged += OnSceneChanged; } 
    void OnDisable() { SceneManager.activeSceneChanged -= OnSceneChanged; }
}

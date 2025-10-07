using UnityEngine;

public class FlipCamera : MonoBehaviour
{
    Vector2 targetLocalPoint;
    float lerpSpeed = 10.0f;

    public void Flip()
    {
        targetLocalPoint.x *= -1f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetLocalPoint = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPoint, Time.deltaTime * lerpSpeed);
    }
}

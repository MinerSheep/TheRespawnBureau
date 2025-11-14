using UnityEngine;

public class Confetti : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-6.0f, 6.0f), Random.Range(5.0f, 8.0f));
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, 360.0f * Time.deltaTime);
    }
}

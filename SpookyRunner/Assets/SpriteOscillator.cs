using UnityEngine;

public class SpriteOscillator : MonoBehaviour
{
    [SerializeField] float amplitude = 1.0f; // how high it moves
    [SerializeField] float frequency = 2.0f; // how fast it moves

    float currentOffset;



    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position -= new Vector3(0f, currentOffset, 0f);
        currentOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position += new Vector3(0f, currentOffset, 0f);
    }
}

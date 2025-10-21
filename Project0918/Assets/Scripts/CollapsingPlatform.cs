using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    public float timerMax;
    [SerializeField] private float tickRate;
    public bool playerCollided;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCollided = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollided == true)
        {
            timerMax -= tickRate;
        }

        if (timerMax <= 0)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class CollectibleContainer : MonoBehaviour
{

    private bool playerCollision;
    private bool boxOpened;
    public SpriteRenderer spriteRenderer;
    public GameObject collectible;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            OpenBox();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && boxOpened == false)
        {
            playerCollision = true;
            spriteRenderer.color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && boxOpened == false)
        {
            playerCollision = false;
            spriteRenderer.color = Color.white;
        }
    }

    public void OpenBox()
    {
        if (playerCollision == true && boxOpened == false)
        {
            Vector3 boxtransform = transform.position;
            Instantiate(collectible, boxtransform, Quaternion.identity);
            spriteRenderer.color = Color.white;
            boxOpened = true;
        }
    }

}

using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    public bool canDoPuzzle = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canDoPuzzle = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canDoPuzzle = false;
        }
    }

    private void Update()
    {
        
    }
}

using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int DamageAmount=1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playercontroller = collision.gameObject.GetComponent<PlayerController>();
            playercontroller.GetComponent<Health>().TakeDamage(DamageAmount);

            Debug.Log("Obstacle Hit");
        }
    }
}

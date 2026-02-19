using UnityEngine;

public class MinionStats : MonoBehaviour
{
    private float Health = 100;

    public bool IsEnemy = false;

    [Header("Properties")]
    [SerializeField] float MaxHealth = 20;
    [SerializeField] public float Speed = 5;       // movement speed
    [SerializeField] float Weight = 5;      // the higher the weight the less knockback they recieve

    [Header("Personality")]
    [SerializeField] public float Agression;    // how far forward the AI is willing to push
    [SerializeField] public float Variablity;   // how often the AI changes it's position when it strafes
    [SerializeField] public float Focus;        // how acurate the AI is
    [SerializeField] public float Caution;      // how much the AI strafes left and right

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Destroy(gameObject);
        }
    }
}

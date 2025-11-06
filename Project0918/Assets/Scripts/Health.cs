using UnityEngine;
//This script is the general HP system for every item with a HP in the game
public class Health : MonoBehaviour
{
    public int MaxHP = 3;
    public int MinHP = 0;
    public int CurrentHP = 3;
    public bool IsPlayer = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(gameObject.TryGetComponent<PlayerController>(out PlayerController pC))
        {
            IsPlayer=true;
        }
        else
        {
            IsPlayer=false;
        }
    }
    public void TakeDamage(int DamageAmount)
    {
        CurrentHP=Mathf.Clamp(CurrentHP-DamageAmount,MinHP,MaxHP);
        HPUpdate();
        if(IsPlayer)
        {

        }
    }

    public void Heal(int HealAmount)
    {
        CurrentHP=Mathf.Clamp(CurrentHP+HealAmount,MinHP,MaxHP);
        HPUpdate();
        if(IsPlayer)
        {

        }
    }
    private void HPUpdate()
    {
        //This is the placeholder for update on a small HP bar
    }
}

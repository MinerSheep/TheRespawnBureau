using UnityEngine;
//This script is the general HP system for every item with a HP in the game
public class Health : MonoBehaviour
{
    public int MaxHP = 3;
    public int MinHP = 0;
    public int CurrentHP = 3;
    public bool IsPlayer = true;
    public HUD PlayerHud;
    private PlayerController pC;
    // Start is called once before the first execution of Update after the MonoBehaviour is createdk
    void Start()
    {
        if(gameObject.TryGetComponent<PlayerController>(out PlayerController pC))
        {
            IsPlayer=true;
            pC=gameObject.GetComponent<PlayerController>();
        }
        else
        {
            IsPlayer=false;
        }
    }
    public void TakeDamage(int DamageAmount)
    {
        if (IsPlayer && pC.iFrames > 0)
        {
            CurrentHP = Mathf.Clamp(CurrentHP + DamageAmount, MinHP, MaxHP);
            HPUpdate();
            PlayerHud.UpdateHealthAmount();
        }
        else if (!IsPlayer)
        {
            CurrentHP = Mathf.Clamp(CurrentHP + DamageAmount, MinHP, MaxHP);
            HPUpdate();
        }
    }

    public void Heal(int HealAmount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + HealAmount, MinHP, MaxHP);
        HPUpdate();
        if(IsPlayer)
        {
            PlayerHud.UpdateHealthAmount();
        }
    }
    private void HPUpdate()
    {
        //This is the placeholder for update on a small HP bar or animation
    }
}

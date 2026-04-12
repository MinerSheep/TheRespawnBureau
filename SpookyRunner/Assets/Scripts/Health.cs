using UnityEngine;
using System.Collections;
//This script is the general HP system for every item with a HP in the game
public class Health : MonoBehaviour
{
    public int MaxHP = 3;
    public int MinHP = 0;
    public int CurrentHP = 3;
    public bool IsPlayer = true;
    public float DamageFlashDuration = 0.5f;
    public HUD PlayerHud;
    private PlayerController pC;
    // Start is called once before the first execution of Update after the MonoBehaviour is createdk
    void Start()
    {
        pC = GetComponent<PlayerController>();
        IsPlayer = pC != null;
    }
    public void TakeDamage(int DamageAmount)
    {
        if (IsPlayer)
        {
            if(pC.iFrames>0)
                Debug.Log("Soak " + DamageAmount + " due to iFrames");
            else
            {
                CurrentHP = Mathf.Clamp(CurrentHP - DamageAmount, MinHP, MaxHP);
                HPUpdate();
                PlayerHud.UpdateHealthAmount();
                Debug.Log(DamageAmount);

                StartCoroutine(DamageFlashEffect());

                AudioManager.instance.PlaySound("playerdamage");
            }
        }
        else if (!IsPlayer)
        {
            CurrentHP = Mathf.Clamp(CurrentHP - DamageAmount, MinHP, MaxHP);
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

        // Check if the player has run out of health and kill them
        if(CurrentHP <= 0)
        {
            PlayerEvents.OnPlayerDeath?.Invoke();
        }
    }


    IEnumerator DamageFlashEffect()
    {
        float timer = 0.0f;

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        while (timer < DamageFlashDuration)
        {
            timer += Time.deltaTime;
            
            if (timer < DamageFlashDuration / 2)
            {
                foreach (var renderer in renderers)
                    renderer.color = Color.Lerp(Color.white, Color.red, timer / DamageFlashDuration);
            }
            else
            {
                foreach (var renderer in renderers)
                    renderer.color = Color.Lerp(Color.red, Color.white, timer / DamageFlashDuration);
            }

            yield return null;
        }
    }
}

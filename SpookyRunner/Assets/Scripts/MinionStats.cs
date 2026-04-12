using UnityEngine;
using System.Collections;

public class MinionStats : MonoBehaviour
{
    private float Health = 100;

    public bool IsEnemy = false;
    public float DamageFlashDuration = 0.5f;

    [Header("Properties")]
    [SerializeField] float MaxHealth = 20;
    [SerializeField] public float Speed = 5;       // movement speed
    [SerializeField] float Weight = 5;      // the higher the weight the less knockback they recieve

    [Header("Personality")]
    [SerializeField] public float Agression;    // how far forward the AI is willing to push
    [SerializeField] public float Variablity;   // how often the AI changes it's position when it strafes
    [SerializeField] public float Focus;        // how acurate the AI is
    [SerializeField] public float Caution;      // how much the AI strafes left and right

    private float defaultAgression;
    private float defaultVariablity;
    private float defaultFocus;
    private float defaultCaution;

    public float GetHealth()
    {
        return Health;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Health = MaxHealth;
    }


    void Awake()
    {
        // Save defaults once
        defaultAgression = Agression;
        defaultVariablity = Variablity;
        defaultFocus = Focus;
        defaultCaution = Caution;
    }

    public void ResetStats()
    {
        Agression = defaultAgression;
        Variablity = defaultVariablity;
        Focus = defaultFocus;
        Caution = defaultCaution;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        StartCoroutine(DamageFlashEffect());
        if (Health < 0)
        {
            gameObject.SetActive(false);
        }
    }


    IEnumerator DamageFlashEffect()
    {
        float timer = 0.0f;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        while (timer < DamageFlashDuration)
        {
            timer += Time.deltaTime;

            if (timer < DamageFlashDuration / 2)
            {
                    renderer.color = Color.Lerp(color, Color.red, timer / DamageFlashDuration);
            }
            else
            {
                    renderer.color = Color.Lerp(Color.red, color, timer / DamageFlashDuration);
            }

            yield return null;
        }
    }
}

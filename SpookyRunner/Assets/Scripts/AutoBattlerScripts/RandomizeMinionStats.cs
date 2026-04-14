using UnityEngine;

public class RandomizeMinionStats : MonoBehaviour
{
    [SerializeField] private MinionStats stats;
    [SerializeField] private ProjectileEmitter emitter;

    [SerializeField] private int totalPoints = 10;
    [SerializeField] private float pointValue = 0.2f;
    [SerializeField] private int BulletUpgradeChance = 10;
    void OnEnable()
    {
        if (stats == null)
            return;
        int roll;

        // Distribute points randomly
        for (int i = 0; i < totalPoints; i++)
        {
            roll = Random.Range(0, 4);

            switch (roll)
            {
                case 0:
                    stats.Agression += pointValue;
                    break;

                case 1:
                    stats.Variablity += pointValue;
                    break;

                case 2:
                    stats.Focus += pointValue;
                    break;

                case 3:
                    stats.Caution += pointValue;
                    break;
            }
        }

        roll = Random.Range(0, BulletUpgradeChance);
        switch (roll)
        {
            case 0:
                emitter.Quantity += 1;
                emitter.Spread = Mathf.Max(5f, emitter.Spread - 2f);
                break;
        }
    }
}
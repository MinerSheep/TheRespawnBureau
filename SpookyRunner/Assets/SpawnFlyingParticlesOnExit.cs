using UnityEngine;

public class SpawnFlyingParticlesOnExit : MonoBehaviour
{
    public float StartSpeed = 0.2f;
    public float quantity = 6;
    public float lifetime = 8.0f;
    public string GoTo;
    public GameObject flyingParticlePrefab;

    public void Spawn()
    {

        GameObject Target = GameObject.Find(GoTo);
        if (Target == null)
        {
            Debug.Log("Error: SpawnFlyingParticlesOnExit faild to find '" + GoTo + "' in the scene");
            return;
        }
        float angleStep = 360.0f / quantity;

        for (int i = 0; i < quantity; i++)
        {
            float angle = i * angleStep;

            GameObject particle = Instantiate(flyingParticlePrefab, transform.position, Quaternion.identity);

            FlyToPoint fly = particle.GetComponent<FlyToPoint>();

            fly.startAngle = angle;
            fly.startSpeed = StartSpeed;
            fly.lifetime = lifetime;
            Vector3 world = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(Target.transform.position));
            fly.finalPosition = Target;
        }
    }

}

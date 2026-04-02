using UnityEngine;

public class DamageVignette : MonoBehaviour
{

    public GameObject DVignette;

    public void PlayDamageVignette()
    {
        DVignette.GetComponent<ParticleSystem>().Play();
    }
}

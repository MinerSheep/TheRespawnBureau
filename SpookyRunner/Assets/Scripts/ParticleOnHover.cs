using UnityEngine;
using UnityEngine.EventSystems;

public class ParticleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ParticleSystem sparkleParticles;

    void Start()
    {
        if (sparkleParticles == null)
            sparkleParticles = transform.GetComponentInChildren<ParticleSystem>();

        sparkleParticles.Stop();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sparkleParticles != null)
            sparkleParticles.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (sparkleParticles != null)
            sparkleParticles.Stop();
    }
}

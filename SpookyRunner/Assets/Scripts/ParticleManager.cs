using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance { get; private set; }

    [Header("References")]
    [HideInInspector] public GameObject JumpEffect;
    [HideInInspector] public GameObject RunningEffect;
    [HideInInspector] List<GameObject> DyingEffects;
    [HideInInspector] public GameObject ActivateRunningEffect;

    [SerializeField] ParticleSystem SpeedEffect;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (instance == null)
        {
            var go = new GameObject("[ParticleManager]");
            instance = go.AddComponent<ParticleManager>();
        }
    }


    void Awake()
    {
        Debug.Log("[ParticleManager] Awake on " + gameObject.scene.name);

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        DyingEffects = new List<GameObject>();

        JumpEffect = Resources.Load<GameObject>("Particle/JumpParticle");
        RunningEffect = Resources.Load<GameObject>("Particle/RunningParticle");

    }


    public void JumpEffectCall(Vector3 position)
    {
        Instantiate(JumpEffect,position,Quaternion.identity);
    }

    public void RunningEffectCall(Vector3 position)
    {
        if(!ActivateRunningEffect)
        {
            ActivateRunningEffect = RunnningEffectCreate(position);
        }
        else
        {
            return;
        }
    }
    
    public void RunningEffectDestory()
    {
        if (ActivateRunningEffect != null)
        {
            // 1️ Detach it from the current GameObject
            ActivateRunningEffect.transform.parent = null;

            // 2️ Get the particle system component
            ParticleSystem ps = ActivateRunningEffect.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                // 3️ Stop it from looping
                var main = ps.main;
                main.loop = false;
                // 4️ Stop emission (so it only plays existing particles until they die)
                ps.Stop();
                // 5️ Optionally destroy it later, after all particles fade out
                Destroy(ActivateRunningEffect, ps.main.duration + ps.main.startLifetime.constantMax);
            }

            // 6️ Clear your reference
            ActivateRunningEffect = null;
        }

    }

    public GameObject RunnningEffectCreate(Vector3 position)
    {
        return Instantiate(RunningEffect, position, Quaternion.identity);
    }


    public void ClearAllDyingEffects()
    {
        foreach (var effect in DyingEffects)
        {
            if (effect != null)
                Destroy(effect);
        }
        DyingEffects.Clear();
    }


    public void SetRunningEffectPosition(Vector3 position)
    {
        if(ActivateRunningEffect)
        {
            ActivateRunningEffect.transform.position = new Vector3(position.x, position.y - 0.7f);
        }
    }
}

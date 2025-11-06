using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager instance;

    public static ParticleManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Try to find the instance in the scene
                instance = FindAnyObjectByType<ParticleManager>(FindObjectsInactive.Exclude);
                if (instance == null)
                {
                    // If still null, log an error
                    //Debug.LogError("Partcile Manager is not initialized. Make sure it's in the scene.");

                    // Create one so callers never get null
                    var go = new GameObject("ParticleManager");
                    instance = go.AddComponent<ParticleManager>(); // Awake will run now

                }
            }
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap() => _ = Instance;

    public GameObject JumpEffect;

    [SerializeField] ParticleSystem SpeedEffect;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }




        instance = this;


        // Prevent this object from being destroyed when changing scenes
        transform.parent = null;
        DontDestroyOnLoad(gameObject);


        JumpEffect = Resources.Load<GameObject>("Particle/JumpParticle");


    }

    public void JumpEffectCall(Vector3 position)
    {
        Instantiate(JumpEffect,position,Quaternion.identity);
    }

}

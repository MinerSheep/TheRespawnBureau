using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class RunnerScene : MonoBehaviour
{
    [Header("Settings")]
    public float StartMovingSpeed = 6f;
    public float EndMovingSpeed = 10f;
    public float DashSpeed = 20f;
    public float ChangeTime = 9000f;
    public float AutoRunnerTimer = 0f;

    public float MinimumMovingSpeed = 6f;
    private bool canDash = true;
    public float dashDuration = 1f;
    private float dashTimer = 0f;

    public HUD hud;
    public GameObject Speedlines;

    public ParticleSystem Speedlines;

    // Private variables
    [HideInInspector] public float MovingSpeed;

    public void DashInLevel()
    {
        Speedlines.SetActive(true);
        if (canDash)
        {
            canDash = false;
            MovingSpeed = DashSpeed;
            dashTimer = dashDuration;
            hud.StaminaAmount -= 15f;
            Speedlines.gameObject.SetActive(true);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovingSpeed = StartMovingSpeed;

        AudioManager.instance.PlayMusic("infinite_runner");

        //SetMaskOnTransform(transform);
    }

    void SetMaskOnTransform(Transform transformer)
    {
        SpriteRenderer sr = transformer.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        TilemapRenderer tr = transformer.GetComponent<TilemapRenderer>();
        if (tr != null)
            tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        foreach (Transform child in transformer)
        {
            SetMaskOnTransform(child);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canDash)
        {
            MovingSpeed = Mathf.Lerp(StartMovingSpeed, EndMovingSpeed, AutoRunnerTimer / ChangeTime);
        }

        AutoRunnerTimer += Time.deltaTime;
        
        transform.position += new Vector3(-MovingSpeed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.R))
        {
            TelemetryManager.instance.DeathReason = "Restart Triggered";
            TelemetryManager.instance.RoundEnd(false);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.L) && SceneManager.GetSceneByName("AR02") != null)
            SceneManager.LoadScene("AR02");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && canDash)
        {
            canDash = false;
            MovingSpeed = DashSpeed;
            dashTimer = dashDuration;
            hud.StaminaAmount -= 15f;
        }

        dashTimer -= Time.deltaTime;

        if (dashTimer < 0)
        {
            dashTimer = 0;
            canDash = true;
            MovingSpeed = StartMovingSpeed;
            Speedlines.SetActive(false);
        }

        if (MovingSpeed < MinimumMovingSpeed)
        {
            MovingSpeed = MinimumMovingSpeed;
            
        }
    }

    void OnDestroy()
    {
        // This only fires if RoundEnd is not called beforehand
        TelemetryManager.instance.DeathReason = "Game Quit";
        TelemetryManager.instance.RoundEnd(false);
        
        AudioManager.instance.StopMusic();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class RunnerScene : MonoBehaviour
{
    [Header("Settings")]
    public float StartMovingSpeed = 6f;
    public float EndMovingSpeed = 10f;
    public float ChangeTime = 9000f;
    public float AutoRunnerTimer = 0f;

    // Private variables
    [HideInInspector] public float MovingSpeed;

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
    void Update()
    {
        AutoRunnerTimer += Time.deltaTime;
        MovingSpeed = Mathf.Lerp(StartMovingSpeed, EndMovingSpeed, AutoRunnerTimer / ChangeTime);
        transform.position += new Vector3(-MovingSpeed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.R))
        {
            TelemetryManager.instance.DeathReason = "Restart Triggered";
            TelemetryManager.instance.RoundEnd(false);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.L))
            SceneManager.LoadScene("AR02");
    }

    void OnDestroy()
    {
        AudioManager.instance.StopMusic();
    }
}

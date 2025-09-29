using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerScene : MonoBehaviour
{
    public float StartMovingSpeed = 6f;
    public float EndMovingSpeed = 10f;
    public float ChangeTime = 9000f;
    public float AutoRunnerTimer=0f;

    public float MovingSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovingSpeed = StartMovingSpeed;

        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AutoRunnerTimer += Time.deltaTime;
        MovingSpeed = Mathf.Lerp(StartMovingSpeed, EndMovingSpeed, AutoRunnerTimer / ChangeTime);
        transform.position += new Vector3(-MovingSpeed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("AutoRunnerTester");
    }
}

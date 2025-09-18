using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject roomLight;
    public static bool isPaused = false;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseGame();
        }
    }
    public void Win()
    {
        _uiManager.Win();
        StartCoroutine(Win(1f));
    }

    public void Lose()
    {
        _uiManager.Lose();
    }

    public static void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }

        else
        {
            Time.timeScale = 1f;
        }
    }

    IEnumerator Win(float second)
    {
        Instantiate(roomLight, Vector3.zero, transform.rotation);
        yield return new WaitForSeconds(second);
        EnemyController[] enemyControllers = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemyController in enemyControllers)
        {
            enemyController.gameObject.SetActive(false);
        }
    }
}

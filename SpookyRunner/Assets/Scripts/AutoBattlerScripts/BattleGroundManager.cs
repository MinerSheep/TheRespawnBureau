using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleGroundManager : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Player;
    [SerializeField] Canvas canvas;
    [SerializeField] StatUpgrade Upgrades;

    public int Rounds = 0;

    enum GameStates
    {
        Entry,
        Build,
        RoundStart,
        RoundEnd,
        Fight
    }

    private GameStates state;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StateChange(GameStates.Entry);
    }

    void Update()
    {
        switch (state)
        {
            case GameStates.Fight:
                if (!Player.activeInHierarchy)
                    BattleEndLose();
                if (!Enemy.activeInHierarchy)
                    BattleEndWin();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Enemy.SetActive(false);
                }
                break;
        }
    }

    void StateChange(GameStates newst)
    {
        state = newst;

        switch (state)
        {
            case GameStates.Build:
                GameObject target = FindChildByName("Build");
                if (target != null)
                    target.SetActive(true);
                Enemy.SetActive(false);
                Player.SetActive(false);
                break;
            case GameStates.RoundStart:
                Rounds++;
                GameObject obj = FindChildByName("Round");
                TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();

                if (text != null)
                {
                    text.text = "Round " + Rounds;
                }

                Player.transform.position = new Vector2(0, -3);
                Enemy.transform.position = new Vector2(0, 3);
                break;
            case GameStates.RoundEnd:
                MinionStats mt = Player.GetComponent<MinionStats>();
                if (mt != null)
                    Upgrades.Points += (int)mt.GetHealth();
                Upgrades.UpdatePointsUI();
                break;
            case GameStates.Entry:
                GameObject open = FindChildByName("Opening");
                open.SetActive(true);
                Enemy.SetActive(false);
                Player.SetActive(false);
                break;
        }
    }
    public void FinishedBuildStage()
    {
        StateChange(GameStates.RoundStart);
        StartCoroutine(StartBattle());
    }

    public void BattleEndWin()
    {
        StateChange(GameStates.RoundEnd);
        StartCoroutine(EndBattle(true));
    }

    public void BattleEndLose()
    {
        StateChange(GameStates.RoundEnd);
        StartCoroutine(EndBattle(false));
    }

    public void EnterBuildMode()
    {
        StateChange(GameStates.Build);
    }

    GameObject FindChildByName(string name)
    {
        foreach (Transform child in canvas.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
                return child.gameObject;
        }
        return null;
    }

    IEnumerator StartBattle()
    {
        GameObject roundObj = FindChildByName("Round");
        GameObject startObj = FindChildByName("Fight");

        if (roundObj != null)
        {
            roundObj.SetActive(true);
            yield return new WaitForSeconds(1f);
            roundObj.SetActive(false);
        }

        if (startObj != null)
        {
            startObj.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            startObj.SetActive(false);
        }

        Enemy.SetActive(true);
        Player.SetActive(true);
        StateChange(GameStates.Fight);
    }

    IEnumerator EndBattle(bool win)
    {
        GameObject target = FindChildByName(win ? "Win" : "Lose");
        AudioManager.instance.PlaySound("Win");
        if (target != null)
        {
            target.SetActive(true);
            yield return new WaitForSeconds(1f);
            target.SetActive(false);
        }
        if (win)
        {
            StateChange(GameStates.Build);
        }
        else
        {
            StateChange(GameStates.Entry);
            Rounds = 0;
        }
    }

    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

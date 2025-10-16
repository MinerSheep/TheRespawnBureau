using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class PuzzleManagerArrowsOrder : MonoBehaviour
{
    //References
    public GameObject PuzzlePanel;
    public GameObject PuzzleTrigger;

    public GameObject door;

    public PuzzleTrigger PT;

    public TextMeshProUGUI TimerText;
    

    [Header("Puzzle Settings")]
    public List<KeyCode> correctOrder = new List<KeyCode>
    {
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow
    };

    public List<GameObject> images = new List<GameObject>
    {
        
    };


    public float timeRemaining = 10f;
    public bool timerRunning = false;

    private int currentStep = 0;
    private bool puzzleActive = false;

    public void Start()
    {
        PuzzlePanel.SetActive(false);
        TimerText.text = timeRemaining.ToString("F1");
    }

    public void Update()
    {
        if(PT.canDoPuzzle && !puzzleActive && Input.GetKeyDown(KeyCode.E))
        {
            PuzzlePanel.SetActive(true);
            puzzleActive = true;
            currentStep = 0;
        }

        if (!puzzleActive) return;
        

        if (Input.GetKeyDown(KeyCode.LeftArrow)) HandleKeyPress(KeyCode.LeftArrow);
        
        if (Input.GetKeyDown(KeyCode.RightArrow)) HandleKeyPress(KeyCode.RightArrow);
        
        if(Input.GetKeyDown(KeyCode.UpArrow)) HandleKeyPress(KeyCode.UpArrow);
        
        if (Input.GetKeyDown(KeyCode.DownArrow)) HandleKeyPress(KeyCode.DownArrow);
        
        
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;
            TimerText.text = timeRemaining.ToString("F1");

            if (timeRemaining <= 0)
            {
                PuzzleFailed();
            }
        }
        else
        {
            TimerText.text = timeRemaining.ToString("F1");
        }


            TimerText.text = timeRemaining.ToString("F1");
        Debug.Log(timeRemaining.ToString());

        if (timeRemaining <= 0)
        {
            currentStep = 0;
            PuzzlePanel.SetActive(false);
            puzzleActive = false;
        }
    }

    public void HandleKeyPress(KeyCode key)
    {
        Debug.Log("Player pressed: " + key + " | Next key: " + correctOrder[currentStep]);

        if(key == correctOrder[currentStep])
        {
            currentStep++;
            images[currentStep - 1].SetActive(false);

            if (currentStep == 1)
            {
                timerRunning = true;
                timeRemaining = 10f;
            }

            if (currentStep >= correctOrder.Count)
            {
                PuzzleSolved();
            }
        }
    }

    public void PuzzleSolved()
    {
        PuzzlePanel.SetActive(false);
        puzzleActive = false;
        door.SetActive(false);
    }

    public void PuzzleFailed()
    {
        timerRunning = false;
        timeRemaining = 10f;
        currentStep = 0;
        puzzleActive = false;
        PuzzlePanel.SetActive(false);

        foreach (var item in images)
        {
            item.SetActive(true);
        }
    }

    
}


using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;



public class PuzzleManagerArrowsOrder : MonoBehaviour
{
    //References
    public GameObject PuzzlePanel;
    public GameObject PuzzleTrigger;

    public GameObject door;

    public PuzzleTrigger PT;

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

    

    private int currentStep = 0;
    private bool puzzleActive = false;

    public void Start()
    {
        PuzzlePanel.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && PT.canDoPuzzle)
        {
            PuzzlePanel.SetActive(true);
            puzzleActive = true;
            currentStep = 0;
        }

        if(!puzzleActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleKeyPress(KeyCode.LeftArrow);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleKeyPress(KeyCode.RightArrow);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleKeyPress(KeyCode.UpArrow);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            HandleKeyPress(KeyCode.DownArrow);
        }
        
    }

    public void HandleKeyPress(KeyCode key)
    {
        Debug.Log("Player pressed: " + key + " | Next key: " + correctOrder[currentStep]);

        if(key == correctOrder[currentStep])
        {
            currentStep++;

            images[currentStep - 1].SetActive(false);

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
}

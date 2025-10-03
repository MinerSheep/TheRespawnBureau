using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{


    public GameObject PuzzlePanel;
    public GameObject PuzzleTrigger;

    public GameObject door;

    public PuzzleTrigger PT;
    

    

    private void Start()
    {
        PuzzlePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && PT.canDoPuzzle)
        {
            PuzzlePanel.SetActive(true);
        }
        Debug.Log("Can player attempt puzzle? " + PT.canDoPuzzle);
    }

    [Header("Assign in inspector")]
    public List<Button> Buttons;

    [Header("Correct order 0-3")]
    public List<int> correctOrder = new List<int>();

    private int currentStep = 0;

    public void Button1Clicked()
    {
        OnButtonClick(0);
    }
    public void Button2Clicked()
    {
        OnButtonClick(1);
    }
    public void Button3Clicked()
    {
        OnButtonClick(2);
    }
    public void Button4Clicked()
    {
        OnButtonClick(3);
    }

    private void OnButtonClick(int index)
    {
        if (index == correctOrder[currentStep])
        {
            currentStep++;

            if (currentStep >= correctOrder.Count)
            {
                PuzzleSolved();
            }
        }
        else
        {
            PuzzleFailed();
        }
    }

    private void PuzzleSolved()
    {
        PuzzlePanel.SetActive(false);
        ResetPuzzle();
        door.SetActive(false);
    }

    private void PuzzleFailed()
    {
        PuzzlePanel.SetActive(false);
        ResetPuzzle();
    }

    private void ResetPuzzle()
    {
        currentStep = 0;
    }
}

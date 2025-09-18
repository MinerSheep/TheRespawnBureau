using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject TutWASD;
    [SerializeField] private GameObject TutArrow;
    private int controlChoice = 1;
    private void Awake()
    {
        TutArrow.SetActive(false);
        TutWASD.SetActive(false);

        controlChoice = PlayerPrefs.GetInt("controlChoice");
    }
    // Start is called before the first frame update
    void Start()
    {
        if(controlChoice == 1)
        {
            TutWASD.SetActive(true);
        }
        if (controlChoice == 2)
        {
            TutWASD.SetActive(true);
        }
    }
}

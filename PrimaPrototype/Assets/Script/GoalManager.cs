using PrimaF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public int levelunlocked;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GlobalDataStatic.unlockedLevel < levelunlocked)
        {
            GlobalDataStatic.unlockedLevel = levelunlocked;
            Debug.Log(levelunlocked);
        }
    }
}

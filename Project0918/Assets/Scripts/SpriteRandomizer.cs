using System.Collections.Generic;
using UnityEngine;

// This component is to be used so that objects spawned in will randomize their appearance
public class SpriteRandomizer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] List<Sprite> spriteList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        System.Random r = new System.Random();

        if (spriteList.Count > 0)
            GetComponent<SpriteRenderer>().sprite = spriteList[r.Next(spriteList.Count)];
    }

}

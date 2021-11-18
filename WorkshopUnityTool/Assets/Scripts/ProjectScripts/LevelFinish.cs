using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //FinishedLevel
        print("LevelFInished");

        //Update quest
        GameManager.Instance.questManager.UpdateQuests();

        //Load NextLevel (+ move player) 
        GameManager.Instance.levelManager.LoadNextLevel();
    }
}

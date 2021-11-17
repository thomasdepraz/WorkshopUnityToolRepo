using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{ 
    [Header("References")]
    public QuestDatabase questDatabase;
    public List<Quest> currentQuests = new List<Quest>();
    public TextMeshProUGUI questDescription;

    //private variable
    List<Quest> cachedQuests = new List<Quest>();



    // Start is called before the first frame update
    void Start()
    {
        //Init cached quests
        for (int i = 0; i < questDatabase.allQuests.Length; i++)
        {
            cachedQuests.Add(questDatabase.allQuests[i]);
        }

        //Pick first 3 quests
        for (int i = 0; i < 3; i++)
        {
            currentQuests.Add(cachedQuests[0]);
            cachedQuests.RemoveAt(0);
        }

        DisplayQuests();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            UpdateQuests();
    }

    void UpdateQuests()
    {
        List<Quest> clearList = new List<Quest>();
        for (int i = 0; i < currentQuests.Count; i++)
        {
            if(isQuestFinished(currentQuests[i]))
            {
                //Give Reward
                switch (currentQuests[i].rewardType)
                {
                    case RewardType.Coins:
                        //Give Coins
                        break;
                    case RewardType.Score:
                        GameManager.Instance.score += currentQuests[i].rewardValue;
                        print("Score " + currentQuests[i].rewardValue.ToString());
                        break;
                    case RewardType.Exp:
                        //Give Coins
                        break;
                    default:
                        break;
                }
                clearList.Add(currentQuests[i]);
            }
        }

        //Clear and refill
        int count = clearList.Count;
        for (int i = 0; i < count; i++)
        {
            currentQuests.Remove(clearList[i]);
        }
        for (int i = 0; i < count; i++)
        {
            if(cachedQuests.Count >0)
            {
                currentQuests.Add(cachedQuests[0]);
                cachedQuests.RemoveAt(0);
            }
        }

        //Update Quest Description;
        DisplayQuests();
    }

    bool isQuestFinished(Quest quest)
    {
        switch (quest.challengeType)
        {
            case ChallengeType.Kill:
                if(GameManager.Instance.currentkillCount >= quest.challengeValue)
                {
                    return true;
                }
                break;

            case ChallengeType.Speed:
                if(GameManager.Instance.currentLevelTimer < (float)quest.challengeValue)
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    public void DisplayQuests()
    {
        questDescription.text = "Quests : ";
        for (int i = 0; i < currentQuests.Count; i++)
        {
            questDescription.text += (currentQuests[i].questLine + " // "); 
        }
    }
}

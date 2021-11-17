using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Quest
{
    public string name;
    public string questLine;
    public int challengeValue;
    public int rewardValue;
    public RewardType rewardType;
    public ChallengeType challengeType;
}

public enum RewardType {Coins, Score, Exp}
public enum ChallengeType {Kill, Speed}
[CreateAssetMenu(fileName = "New Quest Database", menuName = "Quest Database", order = 100)]
public class QuestDatabase : ScriptableObject
{
    public Quest[] allQuests;

#if UNITY_EDITOR
    public TextAsset csvFile;
#endif
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestDatabase))]
public class QuestDatabaseInspector : Editor
{
    QuestDatabase script;

    private void OnEnable()
    {
        script = target as QuestDatabase;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        if (script.csvFile == null) return;

        if(GUILayout.Button("Parse CSV"))
        {
            List<string[]> csvContents = ParseCSV();
            script.allQuests = GenerateQuests(csvContents);
            EditorUtility.SetDirty(script);
        }
    }

    private Quest[] GenerateQuests(List<string[]> csvContents)
    {
        if (csvContents == null) return null;
        
        Quest[] result = new Quest[csvContents.Count];
        if(csvContents.Count == 0) return result;

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = GenerateQuest(csvContents[i]);
        }


        return result;
    }

    private Quest GenerateQuest(string[] content)
    {
        Quest result = new Quest();

        result.name = content[1];

        result.questLine = content[2];

        int.TryParse(content[3], out result.challengeValue);
        int.TryParse(content[4], out result.rewardValue);

        switch (content[5])
        {
            case "Score": result.rewardType = RewardType.Score; break;
            case "Pièces": result.rewardType = RewardType.Coins; break;
            case "Exp": result.rewardType = RewardType.Exp; break;
            default:
                break;
        }

        switch (content[6])
        {
            case "Kill\r": result.challengeType = ChallengeType.Kill; break;
            case "Speed\r": result.challengeType = ChallengeType.Speed; break;
            default:
                break;
        }

        //Veleurs dynamique 
        result.questLine = result.questLine.Replace("$number", content[3]);
        return result;
    }

    private List<string[]> ParseCSV()
    {
        string rawContent = script.csvFile.text;

        char[] separators = new char[] { '\n'};
        string[] lines = rawContent.Split(separators, StringSplitOptions.None);

        List<string[]> result = new List<string[]>();

        string[] cellSeparator = new string[] { "\t" };
        for (int i = 1; i < lines.Length; i++)
        {
            string[] cells = lines[i].Split(cellSeparator, StringSplitOptions.None);
            result.Add(cells);
        }
        return result;
    }
}

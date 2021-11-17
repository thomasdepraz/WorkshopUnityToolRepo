using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

    public TextAsset localizationCSV;
    public Dictionary<string, string> mainDictionary;
    public int currentLanguage;

    public event Action OnLanguageChange; 

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        mainDictionary = new Dictionary<string, string>();

        //parse le csv
        List<string[]> csvContent = CSVUtility.ParseCSV(localizationCSV);

        //attribuer les clés
        for (int i = 0; i < csvContent.Count; i++)
        {
            string[] curLine = csvContent[i];
            for (int j = 1; j < curLine.Length; j++)
            {
                mainDictionary.Add(curLine[0] + "_" + (j - 1).ToString(), curLine[j]);
            }
        }

    }

    private IEnumerator Start()
    {
        yield return null;
        ChangeLanguage(0);
    }

    public void ChangeLanguage(int newLanguage)
    {
        currentLanguage = newLanguage;
        if (OnLanguageChange != null) OnLanguageChange();
    }

    public string FetchText(string localizationKey)
    {
        return mainDictionary[localizationKey + "_" + currentLanguage.ToString()];
    }
}

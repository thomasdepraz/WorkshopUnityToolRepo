using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LocalizeMe : MonoBehaviour
{
    public TextMeshProUGUI selfText;
    public string localizationKey;

    public UnityEvent onRefreshText;

    private void Start()
    {
        LocalizationManager.instance.OnLanguageChange += RefreshText;
    }

    public void RefreshText()
    {
        selfText.text = LocalizationManager.instance.FetchText(localizationKey);

        onRefreshText.Invoke();
    }
}

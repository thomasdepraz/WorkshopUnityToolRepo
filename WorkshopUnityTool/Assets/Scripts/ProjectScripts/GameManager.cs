using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
   
    public static GameManager Instance;

    [Header("References")]
    public MenuManager menuManager;
    public QuestManager questManager;
    public CharacterController2D playerController;
    public LevelManager levelManager;

    [Header("Feedback References")]
    public Screenshake shake;
    public Screenshake knockbackShake;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI timerText;

    int _currentkillCount = 0;
    [HideInInspector] public int currentkillCount
    {
        get => _currentkillCount;
        set
        {
            _currentkillCount = value;
            killCountText.text = "KillCount : " + _currentkillCount.ToString();
        }
    }
    public int score;

    //prvate variables
    private float timer;
    private float slowMoTimer = 0;
    private Coroutine slowMotion;

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
            Instance = this;
    }

    private IEnumerator SlowMotion(float targetSpeed, float duration, float easeInSpeed, float easeOutSpeed)
    {
        slowMoTimer = 0;
        while (Time.timeScale > targetSpeed)
        {
            slowMoTimer += Time.fixedDeltaTime * easeInSpeed;
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetSpeed, slowMoTimer);
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = targetSpeed;

        yield return new WaitForSecondsRealtime(duration);

        slowMoTimer = 0;
        while (Time.timeScale <= 1)
        {
            slowMoTimer += Time.fixedDeltaTime * easeOutSpeed;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, slowMoTimer);
            yield return new WaitForEndOfFrame();
        }

        slowMoTimer = 0;
        Time.timeScale = 1;
    }
}

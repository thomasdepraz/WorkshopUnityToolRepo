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
    public BulletPoolManager enemyBullets;

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

    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            timer += Time.deltaTime;
            if (timer >= 2) timer = 2;

            if(timer < 2 && slowMotion == null)
            {
                slowMotion = StartCoroutine(SlowMotion(0.5f, 3, 1, 1));
            }
            if(timer >= 2 && slowMotion!=null)
            {
                StopCoroutine(slowMotion);
                StartCoroutine(SlowMotion(1, 3, 1, 1));
                slowMotion = null;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer < 0) timer = 0;

            print(timer);
        }
        if(Input.GetMouseButtonUp(1))
        {
            if(timer < 2)
            {
                if(slowMotion != null)
                {
                    StopCoroutine(slowMotion);
                    StartCoroutine(SlowMotion(1, 3, 1, 1));
                }
                slowMotion = null;
            }
            
        }
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

        /*yield return new WaitForSecondsRealtime(duration);

        slowMoTimer = 0;
        while (Time.timeScale <= 1)
        {
            slowMoTimer += Time.fixedDeltaTime * easeOutSpeed;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, slowMoTimer);
            yield return new WaitForEndOfFrame();
        }

        slowMoTimer = 0;
        Time.timeScale = 1; */
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
   
    public static GameManager Instance;

    [Header("References")]
    public MenuManager menuManager;
    public QuestManager questManager;
    public CharacterController2D playerController;

    [Header("Feedback References")]
    public Screenshake shake;
    public Screenshake knockbackShake;

    [HideInInspector] public int currentkillCount;
    [HideInInspector] public float currentLevelTimer;
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

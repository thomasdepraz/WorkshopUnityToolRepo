using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public TextMeshProUGUI scoreText;
    public Image slowMotionFillBar;

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
    int _score = 0;
    [HideInInspector]public int score
    {
        get => _score;
        set
        {
            _score = value;
            scoreText.text = "Score : " + _score.ToString();
        }
    }



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
        if(Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }

    public List<ChunkProfile> ShuffleLevels(List<ChunkProfile> levelList) //FisherYates Shuffle
    {
        int n = levelList.Count;
        for (int i = 0; i < (n - 1); i++)
        {
            int r = i + Random.Range(0, n - i);
            ChunkProfile t = levelList[r];
            levelList[r] = levelList[i];
            levelList[i] = t;
        }
        return levelList;
    }
}

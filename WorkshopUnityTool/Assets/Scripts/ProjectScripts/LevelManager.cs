using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public List<ChunkProfile> chunkPool = new List<ChunkProfile>();
    public ObjectPool obstaclePool;
    public ObjectPool enemyPool;
    public GameObject playerStart;
    public LevelFinish levelGoal;


    [HideInInspector] public int currentLevelIndex = 0;

    float _levelTimer;
    [HideInInspector] public float levelTimer
    {
        get => _levelTimer;
        set
        {
            _levelTimer = value;
            GameManager.Instance.timerText.text = "Timer : " + ((int)_levelTimer).ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel(chunkPool[0]);
    }

    private void Update()
    {
        levelTimer += Time.deltaTime;
    }

    public void LoadLevel(ChunkProfile levelProfile)
    {
        GameObject go;
        for (int i = 0; i < levelProfile.tiles.Length; i++)
        {
            int y = (int)Mathf.Floor(i / 17);
            int x = i - (y * 17);
            
            switch (levelProfile.tiles[i])
            {
                case TileType.None:
                    break;
                case TileType.Obstacle:
                    //recruit free member
                    go = obstaclePool.GetFreeElement();
                    go.transform.localPosition = new Vector3(x, -y, 0);
                    go.SetActive(true);
                    break;
                case TileType.Enemy:
                    //recruit free member
                    go = enemyPool.GetFreeElement();
                    go.transform.localPosition = new Vector3(x, -y, 0);
                    go.SetActive(true);
                    break;
                case TileType.PlayerSpawner:
                    playerStart.transform.localPosition = new Vector3(x, -y, 0);
                    break;
                case TileType.LevelFinish:
                    levelGoal.gameObject.transform.localPosition = new Vector3(x, -y, 0);
                    break;
                default:
                    break;
            }
        }
        GameManager.Instance.playerController.transform.position = playerStart.transform.position;
        
    }

    public void LoadNextLevel()
    {
        ClearLevel();
        if (currentLevelIndex >= chunkPool.Count -1)
            currentLevelIndex = 0;
        else
            currentLevelIndex++;

        levelTimer = 0;
        GameManager.Instance.currentkillCount = 0;
        LoadLevel(chunkPool[currentLevelIndex]);
    }

    void ClearLevel()
    {
        enemyPool.ResetPool();
        obstaclePool.ResetPool();
    }
}

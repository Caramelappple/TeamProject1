using UnityEngine;
using UnityEngine.SceneManagement;

public class NKY_GameManager : MonoBehaviour
{
    public static NKY_GameManager instance;
    public LSO_PlayerMovement player;
    public GameObject EndUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            return;
        }

        LSO_PlayerMovement freshPlayer = FindAnyObjectByType<LSO_PlayerMovement>();

        if (freshPlayer != null)
        {
            LSO_PlayerMovement.instance = freshPlayer;
            DontDestroyOnLoad(freshPlayer.gameObject);
            freshPlayer.gameObject.SetActive(true);
        }

        // 스폰 포인트 위치 동기
        if (LSO_PlayerMovement.instance != null)
        {
            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            if (spawnPoint != null)
            {
                LSO_PlayerMovement.instance.transform.position = spawnPoint.transform.position;
            }
        }
    }
}
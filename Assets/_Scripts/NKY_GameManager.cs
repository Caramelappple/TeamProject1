using UnityEngine;
using UnityEngine.SceneManagement;

public class NKY_GameManager : MonoBehaviour
{
    public static NKY_GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    [field:SerializeField] public GameObject player {  get; private set; }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu") return;

        // 현재 씬에 있는 플레이어 컴포넌트 연결 최신화
        LSO_PlayerMovement currentInstance = FindAnyObjectByType<LSO_PlayerMovement>();
        if (currentInstance != null)
        {
            LSO_PlayerMovement.instance = currentInstance;
        }

        // 스폰 포인트 위치로 플레이어 이동
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint != null && LSO_PlayerMovement.instance != null)
        {
            LSO_PlayerMovement.instance.transform.position = spawnPoint.transform.position;
            Debug.Log("[매니저] 플레이어를 SpawnPoint 위치로 이동시켰습니다.");
        }
    }

    public void SaveCurrentStatus()
    {
        // 연동 해제로 인해 비워둠
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
public class NKY_GameManager : MonoBehaviour
{
    public static NKY_GameManager instance;
    
    [SerializeField] private NKY_SoundData[] soundData;

    // ★ 보스가 그토록 원하던 바로 그 player 변수입니다!
    // 보스 코드가 이 변수를 참조하므로, 씬이 바뀔 때마다 최신 플레이어로 갈아끼워 줍니다.
    [field: SerializeField] public GameObject player { get; private set; }

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
            // 메인 메뉴로 오면 정적 주소록과 내부 플레이어 변수를 깔끔하게 비웁니다.
            LSO_PlayerMovement.instance = null;
            player = null;

            ClearInGameDontDestroyObjects();
            return;
        }

        // [핵심 1] 새로운 씬이 열리자마자 씬에 새로 태어난 진짜 플레이어 오브젝트를 찾습니다.
        LSO_PlayerMovement currentInstance = FindAnyObjectByType<LSO_PlayerMovement>();
        if (currentInstance != null)
        {
            // 1. static 주소록 갱신
            LSO_PlayerMovement.instance = currentInstance;

            // 2. ★ [가장 중요] 보스가 참조하는 GameManager 내의 player 변수에 새 플레이어 게임 오브젝트를 쏙 집어넣어 줍니다!
            player = currentInstance.gameObject;

            Debug.Log($"[매니저] 보스가 사용할 플레이어 오브젝트({player.name}) 바인딩 완벽 성공!");
        }

        // [핵심 2] 스폰 포인트 위치로 플레이어 이동
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint != null && LSO_PlayerMovement.instance != null)
        {
            LSO_PlayerMovement.instance.transform.position = spawnPoint.transform.position;
        }
        
        if (scene.name != "MainMenu" &&  scene.name != "KHG_tumap")
        {
            NKY_SoundManager.Instance.PlayBGM(soundData[Random.Range(0, soundData.Length)].soundName); 
            Debug.Log("dfa");
        }
    }

    private void ClearInGameDontDestroyObjects()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                // 1. 필수적인 기본 매니저들 이름 예외 처리
                if (obj.name == "GameManager" ||
                    obj.name == "SoundManager" ||
                    obj.name == "[Debug Updater]")
                {
                    continue;
                }
                if (obj.GetComponentInChildren<KDH_SceneFader>(true) != null ||
                    obj.GetComponentInParent<KDH_SceneFader>(true) != null)
                {
                    continue;
                }

                // 위 조건에 안 걸리는 진짜 인게임 요소들(Player, UI 등)만 깔끔하게 삭제합니다.
                Destroy(obj);
            }
        }
        Debug.Log("[매니저] 메인 메뉴 진입: 페이더를 제외한 인게임 무적 오브젝트 소각 완료!");
    }
}
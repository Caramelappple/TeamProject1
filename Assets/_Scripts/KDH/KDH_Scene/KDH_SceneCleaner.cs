using UnityEngine;

public class KDH_SceneCleaner : MonoBehaviour
{
    private void Awake()
    {
        // 1. 데이터 및 싱글톤 연결고리 완전히 초기화
        PlayerPrefs.DeleteAll();
        LSO_PlayerMovement.instance = null;
        NKY_GameManager.instance = null;
        Debug.Log("[청소부] 싱글톤 고리 및 데이터 초기화 완료");

        // 2. DontDestroyOnLoad 구역의 찌꺼기들 소각 시작
        CleanUpDontDestroyOnLoad();
    }

    private void CleanUpDontDestroyOnLoad()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            // DontDestroyOnLoad 구역(-1)에 있는 최상위 오브젝트들을 검사
            if (obj.transform.parent == null && obj.scene.buildIndex == -1)
            {
                // 대소문자 실수를 방지하기 위해 오브젝트 이름을 전부 소문자로 변환합니다.
                string nameLower = obj.name.ToLower();

                // 메인 메뉴 유지에 필수적인 페이더(SceneFader)는 절대 지우면 안 됩니다!
                if (nameLower.Contains("fader") || obj.GetComponent<KDH_SceneFader>() != null)
                {
                    continue;
                }

                // ★ [핵심 수정] 이름에 player(cat_Player 포함), gamemanager, boss, ui 등이 들어가면 즉시 파괴!
                if (nameLower.Contains("player") ||
                    nameLower.Contains("gamemanager") ||
                    nameLower.Contains("soundmanager") ||
                    nameLower.Contains("boss") ||
                    nameLower.Contains("ui") ||
                    nameLower.Contains("canvas")) // Canvas 찌꺼기까지 확실하게 저격
                {
                    Debug.Log($"[청소부 소각 성공] : {obj.name}를 파괴했습니다.");
                    Destroy(obj);
                }
            }
        }
    }
}
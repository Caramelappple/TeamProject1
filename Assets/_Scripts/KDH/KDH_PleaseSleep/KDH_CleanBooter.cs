using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_CleanBooter : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("[시스템] 완전 초기화 포맷 시퀀스 시작...");

        // 1. 모든 정적 데이터 및 싱글톤 고리 완전히 박살내기
        PlayerPrefs.DeleteAll();
        LSO_PlayerMovement.instance = null;
        NKY_GameManager.instance = null;

        // 2. DontDestroyOnLoad 구역을 포함하여 씬에 존재하는 "모든" 오브젝트를 리스트로 확보
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            // 이 청소 코드를 돌리고 있는 자기 자신(Booter)과 카메라 빼고 "전부 다" 즉시 파괴
            if (obj != gameObject && obj.transform.root.gameObject != gameObject)
            {
                // 페이더(SceneFader) 마저도 완전히 지워버립니다. (재부팅이니까요!)
                DestroyImmediate(obj);
            }
        }

        Debug.Log("[시스템] 청소 완료. 메인 메뉴로 깨끗하게 진입합니다.");

        // 3. 완벽하게 포맷된 청정 상태에서 메인 메뉴 씬 로드
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
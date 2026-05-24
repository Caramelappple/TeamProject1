using Unity.Cinemachine;
using UnityEngine;

public class KDH_InGame1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. [순서 변경] 현재 플레이어의 체력(Value)을 데이터 매니저에 먼저 안전하게 백업합니다.
            if (NKY_GameManager.instance != null)
            {
                NKY_GameManager.instance.SaveCurrentStatus();
            }

            // 2. 체력 저장이 완전히 끝난 직후, 화면을 가리며 다음 씬("InGame")으로 이동합니다.
            if (KDH_SceneFader.Instance != null)
            {
                KDH_SceneFader.Instance.FadeToScene("InGame");
            }
        }
    }
}
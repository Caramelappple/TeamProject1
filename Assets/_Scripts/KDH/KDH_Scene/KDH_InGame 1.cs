using Unity.Cinemachine;
using UnityEngine;

public class KDH_InGame1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            // 2. 체력 저장이 완전히 끝난 직후, 화면을 가리며 다음 씬("InGame")으로 이동합니다.
            if (KDH_SceneFader.Instance != null)
            {
                KDH_SceneFader.Instance.FadeToScene("InGame");
            }
        }
    }
}
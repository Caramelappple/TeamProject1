using UnityEngine;

public class KDH_ItemBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 매니저 스크립트를 찾음
            KDH_SkillUpdate manager = Object.FindFirstObjectByType<KDH_SkillUpdate>();

            if (manager != null)
            {
                // 이 함수 안에서 UI도 켜고, 스킬도 랜덤으로 뽑음
                manager.ShowSkillSelection();

                // 한 번 먹으면 사라짐
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("씬에 KDH_SkillUpdate 매니저가 없음");
            }
        }
    }
}
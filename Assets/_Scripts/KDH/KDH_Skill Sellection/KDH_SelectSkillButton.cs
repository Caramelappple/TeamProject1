using System;
using UnityEngine;

public class KDH_SelectSkillButton : MonoBehaviour
{
    [SerializeField] private KDH_SkillCardUI skillCardUI;
    [SerializeField] private Transform parent;
    public void OnNewSkill()
    {
        Debug.Log("버튼이 클릭되었습니다!");

        if (skillCardUI == null)
        {
            Debug.LogError("오류: skillCardUI가 할당되지 않았습니다!");
            return;
        }

        if (skillCardUI.mySkillData == null)
        {
            Debug.LogError("오류: 현재 카드에 스킬 데이터가 없음");
            return;
        }

        if (skillCardUI.mySkillData.skillIConPrefabs == null)
        {
            Debug.LogError("오류: 스킬 데이터 내부에 프리팹(아이콘)이 등록되지 않았습니다!");
            return;
        }

        // 실제 생성 로직
        GameObject newSkill = Instantiate(skillCardUI.mySkillData.skillIConPrefabs, parent);
        Debug.Log("프리팹 생성 성공: " + newSkill.name);
    }
}

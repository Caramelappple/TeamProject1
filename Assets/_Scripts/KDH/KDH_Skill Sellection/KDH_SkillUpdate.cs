using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class KDH_SkillUpdate : MonoBehaviour
{
    public List<KDH_SkillData> allSkills;      // AllSkills 칸
    public List<KDH_SkillData> hadSkillData;   // HadSkillData 칸

    public KDH_SkillCardUI skillCardUI1;       // SkillCard UI1 칸
    public KDH_SkillCardUI skillCardUI2;       // SkillCard UI2 칸
    public GameObject selectionPanel;          // Selection Panel 칸

    public Transform skillBarParent;           // SkillBar Parent 칸

    // 노란 오브젝트 충돌 시 이 함수를 호출
    public void ShowSkillSelection()
    {
        // 이미 배운 스킬 제외하고 리스트 만들기
        List<KDH_SkillData> availableSkills = allSkills.Except(hadSkillData).ToList();

        if (availableSkills.Count < 2) return;

        // 랜덤으로 2개 뽑기
        List<KDH_SkillData> chosen = availableSkills.OrderBy(x => Random.value).Take(2).ToList();

        // UI 셋업
        skillCardUI1.SetUp(chosen[0], this);
        skillCardUI2.SetUp(chosen[1], this);

        // 패널 켜기 및 시간 정지
        selectionPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SelectSkill(KDH_SkillData selected)
    {
        hadSkillData.Add(selected);

        // 스킬바에 프리펩 생성
        if (selected.skillIConPrefabs != null)
        {
            Instantiate(selected.skillIConPrefabs, skillBarParent);
        }

        // 패널 끄고 게임 재개
        selectionPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
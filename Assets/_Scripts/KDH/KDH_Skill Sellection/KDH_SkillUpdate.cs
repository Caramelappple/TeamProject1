using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class KDH_SkillUpdate : MonoBehaviour
{
    public List<KDH_SkillData> allSkills;      // AllSkills 칸
    public List<KDH_SkillData> hadSkillData ;   // HadSkillData 칸
    public KDH_SkillSystem skillSystem;

    public KDH_SkillCardUI skillCardUI1;       // SkillCard UI1 칸
    public KDH_SkillCardUI skillCardUI2;       // SkillCard UI2 칸
    public GameObject selectionPanel;          // Selection Panel 칸

    public Transform skillBarParent;           // SkillBar Parent 칸
    public Transform hadSkillBarParent;

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
    public TextMeshProUGUI globalSkillInfoText; // 인스펙터에서 '스킬데이터 툴' 드래그

    public void SelectSkill(KDH_SkillData selected)
    {
        if (selected == null) return;

        // 배운 스킬 리스트에 추가
        if (!hadSkillData.Contains(selected))
        {
            hadSkillData.Add(selected);
        }

        // 추가된 스킬의 인덱스 값을 가져옴
        int targetIndex = hadSkillData.IndexOf(selected);

        // 스킬바에 아이콘 프리팹 생성
        if (selected.skillIConPrefabs != null)
        {
            // 아이콘
            GameObject iconObj = Instantiate(selected.skillIConPrefabs, skillBarParent);
            GameObject hadIconObj = Instantiate(selected.skillIConPrefabs, hadSkillBarParent);

            iconObj.transform.SetSiblingIndex(targetIndex);
            hadIconObj.transform.SetSiblingIndex(targetIndex);

            KDH_Skill skillScript = iconObj.GetComponent<KDH_Skill>();

            if (skillScript != null)
            {
                // 생성된 아이콘에게 씬에 있는 텍스트 주소를 알려줌
                skillScript.SetTextReference(globalSkillInfoText);

                skillScript.skills = selected.skillPrefab;

                //  리스트와 똑같은 인덱스
                if (skillSystem != null && targetIndex < skillSystem.skills.Length)
                {
                    skillSystem.skills[targetIndex] = skillScript;
                    Debug.Log($"{targetIndex}번 슬롯에 {selected.skillName} 자동 등록 완료!");
                }
            }
        }

        selectionPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
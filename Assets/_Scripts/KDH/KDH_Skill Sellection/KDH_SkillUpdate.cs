using UnityEngine;
using System.Collections.Generic;

public class KDH_SkillUpdate : MonoBehaviour
{
    [SerializeField] private KDH_SkillData[] skills = new KDH_SkillData[4];
    [SerializeField] private KDH_SkillCardUI skillCardUI;
    [SerializeField] private KDH_SkillCardUI skillCardUIanother;
    [SerializeField] private int randomSkill;

    // 이미 선택된 스킬의 인덱스를 저장하는 리스트
    [SerializeField] private List<int> selectedIndexes = new List<int>();

    private void OnEnable()
    {
        RollSkill();
    }

    private void OnDisable()
    {
        skillCardUI.mySkillData = null;
    }

    public void RollSkill()
    {
        // 모든 스킬을 다 뽑았다면 초기화
        if (selectedIndexes.Count >= skills.Length)
        {
            selectedIndexes.Clear();
        }

        int safetyNet = 0; // 무한 루프 방지용
        while (safetyNet < 100)
        {
            randomSkill = Random.Range(0, skills.Length);
            KDH_SkillData selectedData = skills[randomSkill];

            // 이전에 이미 선택해서 리스트에 들어있는가? (이전 판 중복 방지)
            // 현재 옆에 있는 카드(skillCardUIanother)에 이미 들어있는 데이터인가? (현재 화면 중복 방지)
            bool isAlreadySelected = selectedIndexes.Contains(randomSkill);
            bool isShowingInOtherCard = (skillCardUIanother.mySkillData == selectedData);

            if (!isAlreadySelected && !isShowingInOtherCard)
            {
                selectedIndexes.Add(randomSkill);
                break;
            }
            safetyNet++;
        }

        ApplySkillToUI();
    }

    private void ApplySkillToUI()
    {
        if (skillCardUI != null && skills.Length > 0)
        {
            KDH_SkillData data = skills[randomSkill];

            skillCardUI.mySkillData = data;
            skillCardUI.descText.text = data.skillDescription;

            // 이미지 업데이트
            if (skillCardUI.iconImage != null)
            {
                skillCardUI.iconImage.sprite = data.skillIcon;
            }
        }
    }

    // 게임을 새로 시작하거나 초기화하고 싶을 때
    public void ResetSelection()
    {
        selectedIndexes.Clear();
    }
}
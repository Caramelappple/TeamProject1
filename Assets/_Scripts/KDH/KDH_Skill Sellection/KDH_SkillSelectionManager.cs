using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SkillSelectionManager : MonoBehaviour
{
    public List<KDH_SkillData> allSkills;      // 전체 스킬 리스트
    public List<KDH_SkillData> acquiredSkills; // 이미 습득한 스킬 리스트

    public SkillSelectSlot slot1;
    public SkillSelectSlot slot2;
    public GameObject selectionUI; // Skill Selection 오브젝트

    public Transform skillBarTransform; // SkillBar 위치
    public KDH_SkillSystem skillSystem;     // 실제 스킬이 돌아가는 시스템 스크립트

    // 노란색 오브젝트와 충돌했을 때 호출될 함수
    public void OpenSelectionUI()
    {
        // 이미 배운 스킬을 제외한 리스트 생성
        List<KDH_SkillData> availableSkills = allSkills.Except(acquiredSkills).ToList();

        if (availableSkills.Count < 2)
        {
            Debug.Log("더 이상 배울 스킬이 없습니다.");
            return;
        }

        // 랜덤하게 2개 추출 (중복 없이)
        List<KDH_SkillData> pickedSkills = availableSkills.OrderBy(x => Random.value).Take(2).ToList();

        // UI 셋업
        slot1.Setup(pickedSkills[0], this);
        slot2.Setup(pickedSkills[1], this);

        selectionUI.SetActive(true);
        Time.timeScale = 0f; // 선택하는 동안 게임 일시정지 (선택사항)
    }

    public void SelectSkill(KDH_SkillData selectedData)
    {
        // 리스트에 추가
        acquiredSkills.Add(selectedData);

        // SkillBar에 프리펩 생성
        GameObject newSkillUI = Instantiate(selectedData.skillIConPrefabs, skillBarTransform);

        // 생성된 프리펩의 KDH_Skill 컴포넌트 설정 및 시스템 등록
        KDH_Skill skillComp = newSkillUI.GetComponent<KDH_Skill>();
        if (skillComp != null)
        {
            // 데이터 연동 (이름, 쿨타임 등)
            // skillSystem.AddSkill(skillComp); // SkillSystem에 등록하는 함수가 있다면 호출
        }

        // UI 닫기 게임 게임 시작
        selectionUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
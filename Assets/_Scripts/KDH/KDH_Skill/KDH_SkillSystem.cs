using UnityEngine;
using UnityEngine.UI;

public class KDH_SkillSystem : MonoBehaviour
{
    [SerializeField]
    private GraphicRaycaster graphicRaycaster;

    [Header("스킬의 개수가 배열의 크기보다 더 크면 배열의 크기를 늘리면 돼")]
    [SerializeField]
    public KDH_Skill[] skills; // 스킬 장착창(슬롯)

    private bool _canUseSkill = true; // 시온의 무적상태 bool값

    private void Awake()
    {
        // 오늘 추가된 무적/싱글톤 로직을 완전히 제거했습니다.
    }

    private void Update()
    {
        if (!_canUseSkill) return; // 무적 상태면 함수 탈출

        if (!Input.anyKeyDown) return;

        // KeyAction.KeyCount(현재 4개)만큼 반복하며 키 입력을 확인
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            if (i >= skills.Length) break;

            KeyAction action = (KeyAction)i;
            KeyCode mappedKey = KeySetting.keys[action];

            if (Input.GetKeyDown(mappedKey))
            {
                if (skills[i] != null)
                {
                    skills[i].UseSkill(); // i번째 배열에 있는 스킬 발동
                }
            }
        }
    }

    public void SetCanUseSkill(bool canUse)
    {
        _canUseSkill = canUse;
    }

    public void AddSkill(KDH_Skill newSkill)
    {
        bool isAdded = false;

        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            if (skills[i] == null)
            {
                skills[i] = newSkill;
                Debug.Log($"{i}번 슬롯에 {newSkill.name} 장착 완료!");
                isAdded = true;
                break;
            }
        }

        if (!isAdded)
        {
            Debug.Log("더 이상 스킬을 장착할 공간이 없습니다.");
            Destroy(newSkill.gameObject);
        }
    }

    public void RemoveSkillAndRearrange(int index)
    {
        if (index < 0 || index >= skills.Length) return;

        if (skills[index] != null)
        {
            Destroy(skills[index].gameObject);
            skills[index] = null;
        }

        for (int i = index; i < skills.Length - 1; i++)
        {
            skills[i] = skills[i + 1];
            skills[i + 1] = null;
        }
    }
}
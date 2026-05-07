using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KDH_SkillSystem : MonoBehaviour
{
    [SerializeField]
    private GraphicRaycaster graphicRaycaster;
    [Header ("스킬의 개수가 배열의 크기보다 더 크면 배열의 크기를 늘리면 되")]
    [SerializeField]
    public KDH_Skill[] skills; //스킬 장착창(슬롯) <- 여기서 얻은 스킬을 설정하여 사용하면 되

    private List<RaycastResult> raycastResults;
    private PointerEventData pointerEventData;

    private void Awake()
    {
        raycastResults = new List<RaycastResult>();
        pointerEventData = new PointerEventData(null);
    }

    private void Update()
    {
        if (!Input.anyKeyDown) return;

        // KeyAction.KEYCOUNT(현재 4개)만큼 반복하며 키 입력을 확인
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            // 등록된 스킬 배열 개수보다 넘어가면 검사 중지
            if (i >= skills.Length) break;

            // 현재 순서의 열거형(KeyAction)을 가져옴 (0: CTRL, 1: SHIFT, 2: Q, 3: E)
            KeyAction action = (KeyAction)i;

            // 딕셔너리에서 해당 액션에 할당된 실제 키보드 키(KeyCode)를 꺼내옴
            KeyCode mappedKey = KeySetting.keys[action];

            // 그 단축키가 방금 눌렸다면
            if (Input.GetKeyDown(mappedKey))
            {
                if (skills[i] != null)
                {
                    skills[i].UseSkill(); // i번째 배열에 있는 스킬 발동
                }
            }
        }

        // 2. 스킬 아이콘을 마우스로 클릭해서 스킬 시전
        if (Input.GetMouseButtonDown(0))
        {
            raycastResults.Clear();

            pointerEventData.position = Input.mousePosition;
            graphicRaycaster.Raycast(pointerEventData, raycastResults);

            if (raycastResults.Count > 0)
            {
                if (raycastResults[0].gameObject.TryGetComponent<KDH_Skill>(out var skill))
                {
                    skill.UseSkill();
                }
            }
        }
    }
    // 외부에서 스킬 프리팹 정보를 넘겨받는 메서드
    public void AddSkill(KDH_Skill newSkill)
    {
        bool isAdded = false;

        // KeyAction.KEYCOUNT (4개) 만큼 루프
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            // 빈 자리가 있다면
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
            // 자리가 없으면 생성된 프리팹을 다시 삭제
            Destroy(newSkill.gameObject);
        }
    }
}
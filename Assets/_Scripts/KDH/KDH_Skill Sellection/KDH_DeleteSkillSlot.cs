using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KDH_DeleteSkillSlot : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    [SerializeField] private KDH_SkillUpdate _SkillUpdate;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // 이 함수가 버튼을 눌렀을 때 호출될 함수입니다.
    public void DeleteSkillByIndex(int index)
    {
        // 1. 방어 코드: 인덱스 범위 확인
        if (_SkillUpdate.hadSkillData.Count > index)
        {
            Debug.Log($"{index}번 위치의 스킬 삭제 시작");

            // 2. SkillSystem(배열)의 스킬 제거 및 오브젝트 파괴
            if (_SkillUpdate.skillSystem != null && _SkillUpdate.skillSystem.skills.Length > index)
            {
                if (_SkillUpdate.skillSystem.skills[index] != null)
                {
                    // 실제 씬에 존재하던 스킬 아이콘/로직 오브젝트 제거
                    Destroy(_SkillUpdate.skillSystem.skills[index].gameObject);

                    // 배열 슬롯을 null로 비워줌 (단축키 실행 방지)
                    _SkillUpdate.skillSystem.skills[index] = null;
                }
            }

            // 3. UI(hadSkillBarParent) 상의 아이콘도 제거 (선택 사항)
            // hadSkillBarParent의 자식들도 index 순서대로 배치되어 있다면 아래 코드가 유효합니다.
            if (_SkillUpdate.hadSkillBarParent.childCount > index)
            {
                Destroy(_SkillUpdate.hadSkillBarParent.GetChild(index).gameObject);
            }

            // 4. 데이터 리스트에서 삭제
            _SkillUpdate.hadSkillData.RemoveAt(index);

            // 5. 후속 처리
            this.gameObject.SetActive(false); // 삭제 UI 끄기
            Time.timeScale = 1f;              // 게임 재개

            // 만약 삭제 직후 다시 스킬을 뽑게 하고 싶다면:
            // _SkillUpdate.ShowSkillSelection();
        }
        else
        {
            Debug.LogWarning("삭제하려는 인덱스가 유효하지 않습니다.");
        }
    }
    public void OnUIMan()
    {
        _gameObject.SetActive(true);
    }
}

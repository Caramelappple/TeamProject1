using UnityEngine;

public class KDH_DeleteSkillSlot : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] protected GameObject _skillSelectUI;

    [SerializeField] private KDH_SkillUpdate _SkillUpdate;

    private void Start()
    {
        OffUIMan();
    }

    public void DeleteSkillByIndex(int index)
    {
        if (_SkillUpdate.hadSkillData.Count > index)
        {
            // SkillSystem의 배열 데이터 삭제 및 빈칸 앞으로 당기기
            if (_SkillUpdate.skillSystem != null)
            {
                _SkillUpdate.skillSystem.RemoveSkillAndRearrange(index);
            }

            // UI 아이콘 삭제 (해당 인덱스의 자식 오브젝트 파괴)
            if (_SkillUpdate.skillBarParent.childCount > index)
                Destroy(_SkillUpdate.skillBarParent.GetChild(index).gameObject);

            if (_SkillUpdate.hadSkillBarParent.childCount > index)
                Destroy(_SkillUpdate.hadSkillBarParent.GetChild(index).gameObject);

            // 리스트 데이터 삭제 (리스트는 알아서 앞으로 당겨짐)
            _SkillUpdate.hadSkillData.RemoveAt(index);

            // UI 끄기 
            this.gameObject.SetActive(false);
            Time.timeScale = 1f;

            Debug.Log($"{index}번 스킬 삭제 및 정렬 완료!");
        }
    }

    public void Leave()
    {
        OffUIMan();
        _skillSelectUI.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OnUIMan()
    {
        _gameObject.SetActive(true);
    }

    private void OffUIMan()
    {
        _gameObject.SetActive(false);
    }
}

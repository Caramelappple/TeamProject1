using System;
using TMPro;
using UnityEngine;

public class LSO_SkillSlot : MonoBehaviour
{
    private GameObject _skillPrefab;
    
    private LSO_ISkill _skill;
    private LSO_SkillItem[] _skillItem;

    public int slotIndex = 2;
    private LSO_ISkill[] _skillSlot;

    public static LSO_SkillSlot instance;
    
    private LSO_PlayerMovement _playerMovement;
    private void Awake()
    {
        if (!instance) instance = this;
        _playerMovement = GetComponent<LSO_PlayerMovement>();
        _skillSlot = new LSO_ISkill[slotIndex];
        _skillItem = new LSO_SkillItem[slotIndex];
    }
    
    public void AddSkill(LSO_SkillItem skillItem, int index)
    {
        //한번만 추가할 수 있게 하기
        if (! skillItem.canAdd) return;
        skillItem.canAdd = false;
        
        //안보이게 하기
        skillItem.sprite.sprite = null;
        
        LSO_ISkill skill = skillItem.skill;

        if (skill == null)
        {
            Debug.Log("AddSkill Skill Null");
            return;
        }

        // ✅ 모든 슬롯에서 같은 스킬 구독 해제
        for (int i = 0; i < slotIndex; i++)
        {
            if (_skillSlot[i] == skill)
            {
                _playerMovement.OnSkillEvent[i] -= skill.UseSkill;
                _skillSlot[i] = null;
                _skillItem[i] = null;
            }
        }

        // 해당 슬롯 기존 스킬 구독 해제 및 삭제
        if (_skillSlot[index] != null)
            _playerMovement.OnSkillEvent[index] -= _skillSlot[index].UseSkill;

        RemoveSkill(index);

        _skillItem[index] = skillItem;
        _skillSlot[index] = skill;
        _playerMovement.OnSkillEvent[index] += skill.UseSkill;
    }
    
    
    private void RemoveSkill(int index)
    {
        if (_skillItem[index] != null)
        {
            _skillItem[index].DestroyGroup();
            _skillItem[index] = null;
        }
        _skillSlot[index] = null;
    }
}
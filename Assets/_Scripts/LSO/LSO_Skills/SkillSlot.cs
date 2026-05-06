using System;
using LSO.SkillSystem;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    private GameObject _skillPrefab;
    private ISkill _skill;

    private void Start()
    {
        _skill = _skillPrefab.GetComponent<ISkill>();
    }

    public void SetSkill(ISkill skill)
    {
        _skill = skill;
    }

    public void UseSkill()
    {
        _skill?.UseSkill();
    }
    
    private ISkill GetSkill()
    {
        return _skill;
    }
    
    
}
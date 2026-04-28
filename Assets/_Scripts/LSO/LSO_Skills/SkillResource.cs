using System;
using UnityEngine;

public class SkillResource : MonoBehaviour
{
    [SerializeField] private SkilldataSO _data;
    private string _skillName;
    private string _skillDescription;
    private Sprite _skillIcon;
    private GameObject _skillPrefab;

    private void Awake()
    {
        InitializeSkill();
    }

    public void InitializeSkill()
    {
        _skillName = _data.name;
        _skillDescription = _data.skillDescription;
        _skillIcon = _data.skillIcon;
        _skillPrefab = _data.skillPrefab;
    }

    public void GetSkillResource(SkilldataSO data)
    {
        
    }
}

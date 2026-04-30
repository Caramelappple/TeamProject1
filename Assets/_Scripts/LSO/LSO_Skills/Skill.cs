using System;
using LSO.SkillSystem;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private SkillResourceSO skillData;
    
    private SpriteRenderer _sprite;
    private GameObject _prefab;
    private ISkill _skill;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GameObject skillObj = Instantiate(skillData.skillPrefab);
        _skill = skillObj.GetComponent<ISkill>();
        skillObj.transform.position = gameObject.transform.position;
        
        _sprite.sprite = skillData.skillIcon;
        gameObject.name = skillData.skillName;
        _prefab = skillData.skillPrefab;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SkillSlot.instance.AddSkill(_skill,0);
    }
}

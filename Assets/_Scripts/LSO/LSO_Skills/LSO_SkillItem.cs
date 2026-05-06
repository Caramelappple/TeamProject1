using LSO.SkillSystem;
using UnityEngine;

public class LSO_SkillItem : MonoBehaviour
{
    public LSO_SkillResourceSO skillData;
    
    private SpriteRenderer _sprite;
    private GameObject _prefab;
    public  LSO_ISkill _skill{get; private set;}

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
         GameObject skillObj = Instantiate(skillData.skillPrefab,transform);
         skillObj.transform.position = transform.position;
         
        _skill = skillObj.GetComponent<LSO_ISkill>();
        _sprite.sprite = skillData.skillIcon;
        gameObject.name = skillData.skillName;
        _prefab = skillData.skillPrefab;
    }
}

using LSO.SkillSystem;
using UnityEngine;

public class SkillItem : MonoBehaviour
{
    [SerializeField] private SkillResourceSO skillData;
    
    private SpriteRenderer _sprite;
    private GameObject _prefab;
    public  ISkill _skill{get; private set;}

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
         GameObject skillObj = Instantiate(_prefab, transform);
        _skill = skillObj.GetComponent<ISkill>();
        _sprite.sprite = skillData.skillIcon;
        gameObject.name = skillData.skillName;
        _prefab = skillData.skillPrefab;
    }
}

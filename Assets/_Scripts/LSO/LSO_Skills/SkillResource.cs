using UnityEngine;
using LSO.SkillSystem;

public class SkillResource : MonoBehaviour
{
    [SerializeField] private SkillResourceSO data;
    private string _skillName;
    private string _skillDescription;
    private Sprite _skillIcon;
    private GameObject _skillPrefab;

    [SerializeField]private SpriteRenderer sprite;
    private void Awake()
    {
        InitializeSkill();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void InitializeSkill()
    {
        _skillName = data.name;
        _skillDescription = data.skillDescription;
        _skillIcon = data.skillIcon;
        _skillPrefab = data.skillPrefab;
    }

    //private ISkill GetSkill()
    //{
       // return data.skillPrefab.GetComponent<ISkill>();
    //}
    public SkillResourceSO GetSkillData()
    {
        return data;
    }
    
    private void Start()
    {
        sprite.sprite = data.skillIcon;
    }
}

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
    
    // LSO_SkillItem.cs
    public void DestroyGroup()
    {
        // 스킬 오브젝트를 부모에서 분리해서 살려둠
        if (_skill != null)
        {
            GameObject skillObj = (_skill as MonoBehaviour).gameObject;
            skillObj.transform.parent = null;
            skillObj.SetActive(false); // 안보이게만 해둠
        }
    
        Destroy(transform.parent.gameObject); // 나머지 그룹만 삭제
    }
}

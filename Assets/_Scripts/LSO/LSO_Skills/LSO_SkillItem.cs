using LSO.SkillSystem;
using UnityEngine;

public class LSO_SkillItem : MonoBehaviour
{
    public LSO_SkillResourceSO skillData;

    public bool canAdd = true;
    public SpriteRenderer sprite;
    public GameObject prefab;
    public  LSO_ISkill skill{get; private set;}

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
         GameObject skillObj = Instantiate(skillData.skillPrefab,transform);
         skillObj.transform.position = transform.position;
         
        skill = skillObj.GetComponent<LSO_ISkill>();
        sprite.sprite = skillData.skillIcon;
        gameObject.name = skillData.skillName;
        prefab = skillData.skillPrefab;
    }
    
    public void DestroyGroup()
    {
        if (skill != null)
        {
            GameObject skillObj = (skill as MonoBehaviour).gameObject;
            Destroy(skillObj);
        }

        Destroy(transform.parent?.gameObject ?? gameObject);
    }
}

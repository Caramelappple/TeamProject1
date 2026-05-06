using System.Collections.Generic;
using UnityEngine;

public class KDH_SkillRepository : MonoBehaviour
{
    public List<GameObject> skillInventory = new List<GameObject>();

    private void AddSkill(KDH_SkillData data)
    {
        skillInventory.Add(gameObject);
    }
}

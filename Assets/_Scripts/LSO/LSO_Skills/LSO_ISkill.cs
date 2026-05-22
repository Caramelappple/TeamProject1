using System.Collections;
using UnityEngine;
using LSO.SkillSystem;

public interface LSO_ISkill
{
    public void UseSkill(GameObject player);
    public IEnumerator CoolTime(float time);
}

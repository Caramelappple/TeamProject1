using System.Collections;
using UnityEngine;
using LSO.SkillSystem;

public interface LSO_ISkill
{
    public void UseSkill();
    public IEnumerator CoolTime(float time);
}

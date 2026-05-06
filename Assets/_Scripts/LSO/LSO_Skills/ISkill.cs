using System.Collections;
using UnityEngine;
using LSO.SkillSystem;

public interface ISkill
{
    public void UseSkill();
    public IEnumerator CoolTime(float time);
}

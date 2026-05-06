using UnityEngine;

public class KDH_SkillUse : MonoBehaviour
{
    [SerializeField] private KDH_SkillIConInit kdh_SkillI;

    public void OnClick()
    {
        if (kdh_SkillI != null)
            kdh_SkillI.InitSkillUI();
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "KDH_SkillData", menuName = "Scriptable Objects/KDH_SkillData")]
public class KDH_SkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public float maxCooldownTime;
    public GameObject skillPrefab;
    public GameObject skillIConPrefabs;
}

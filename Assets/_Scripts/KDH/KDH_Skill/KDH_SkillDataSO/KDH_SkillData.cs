using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "KDH_SkillData", menuName = "Scriptable Objects/KDH_SkillData")]
public class KDH_SkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public float maxCooldownTime;
    public GameObject skillPrefab; // 이거는 실제로 게임 씬으로 나올 skill
    public GameObject skillIConPrefabs; // 단지 ICon으로만 보일 GameOject
}

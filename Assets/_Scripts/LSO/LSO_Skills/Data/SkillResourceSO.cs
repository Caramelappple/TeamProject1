using UnityEngine;


namespace LSO.SkillSystem
{
    [CreateAssetMenu(fileName = "SkillDataSo", menuName = "SO/SkillDataSo", order = 0)]
    public class SkillResourceSO : ScriptableObject
    {
        public string skillName;
        public string skillDescription;
        public Sprite skillIcon;
        public GameObject skillPrefab;
    }
}


using System.Collections.Generic;
using UnityEngine;

public class KDH_SkillIConInit : MonoBehaviour
{
    private int skillCount = 4;
    public List<int> skillIndex = new List<int>();
    [SerializeField] private GameObject skillICon;

    public void InitSkillUI()
    {
        // 이미 꽉 찼다면 실행하지 않음
        if (skillIndex.Count >= skillCount) return;

        // 새 스킬 인덱스 추가
        int newIdx = skillIndex.Count;
        skillIndex.Add(newIdx);

        // 아이콘 생성
        Instantiate(skillICon, transform);

        Debug.Log($"스킬 아이콘 생성 완료! 현재 개수: {skillIndex.Count}");
    }
}
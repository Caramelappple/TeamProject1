using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KDH_SkillCardUI : MonoBehaviour
{
    public KDH_SkillData mySkillData;
    public Image iconImage;           // Icon Image 칸
    public TextMeshProUGUI nameText;  // Name Text 칸
    public TextMeshProUGUI descText;  // Desc Text 칸
    public GameObject skillPrefab;

    private KDH_SkillUpdate manager;

    public void SetUp(KDH_SkillData data, KDH_SkillUpdate mgr)
    {
        mySkillData = data;
        manager = mgr;

        if (data != null)
        {
            if (mySkillData != null) skillPrefab = data.skillPrefab; // 스킬 선택창에서 바뀌는 부분 (스킬 Prefab) 
            if (iconImage != null) iconImage.sprite = data.skillIcon; // 스킬 선택창에서 바뀌는 부분 (스킬 아이콘)
            if (nameText != null) nameText.text = data.skillName; // 스킬 선택창에서 바뀌는 부분 (스킬의 이름)
            if (descText != null) descText.text = data.skillDescription; // 스킬 선택창에서 바뀌는 부분 (스킬의 설명)
        }
    }

    public void OnClickCard()
    {
        if (manager != null && mySkillData != null)
        {
            manager.SelectSkill(mySkillData);
        }
    }
}
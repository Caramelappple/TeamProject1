using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KDH_SkillCardUI : MonoBehaviour
{
    public KDH_SkillData mySkillData;
    public Image iconImage;           // Icon Image 칸
    public TextMeshProUGUI nameText;  // Name Text 칸
    public TextMeshProUGUI descText;  // Desc Text 칸

    private KDH_SkillUpdate manager;

    public void SetUp(KDH_SkillData data, KDH_SkillUpdate mgr)
    {
        mySkillData = data;
        manager = mgr;

        if (data != null)
        {
            if (iconImage != null) iconImage.sprite = data.skillIcon; // 스킬 선택창에서 바뀌는 부분
            if (nameText != null) nameText.text = data.skillName; // 스킬 선택창에서 바뀌는 부분
            if (descText != null) descText.text = data.skillDescription; // 스킬 선택창에서 바뀌는 부분
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
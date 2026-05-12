using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectSlot : MonoBehaviour
{
    public TextMeshProUGUI skillNameText; // 스킬 이름
    public TextMeshProUGUI skillDescText; // 스킬 설명
    public Image skillIcon; // 스킬 아이콘

    private KDH_SkillData currentData;
    private SkillSelectionManager manager;

    public void Setup(KDH_SkillData data, SkillSelectionManager mgr) // 스킬 바에 진열하는 함수
    {
        currentData = data;
        manager = mgr;

        // UI 셋업
        skillNameText.text = data.skillName;
        skillDescText.text = data.skillDescription;
        skillIcon.sprite = data.skillIcon;
    }

    public void OnClick() // 선택한 스킬을 알리는 함수
    {
        if (currentData != null && manager != null)
        {
            manager.SelectSkill(currentData);
        }
    }
}
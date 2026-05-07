using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectSlot : MonoBehaviour
{
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescText;
    public Image skillIcon;

    private KDH_SkillData currentData;
    private SkillSelectionManager manager;

    public void Setup(KDH_SkillData data, SkillSelectionManager mgr)
    {
        currentData = data;
        manager = mgr;

        skillNameText.text = data.skillName;
        skillDescText.text = data.skillDescription;
        skillIcon.sprite = data.skillIcon;
    }

    public void OnClick()
    {
        // 스킬 선택 시 매니저에게 신호
        manager.SelectSkill(currentData);
    }
}
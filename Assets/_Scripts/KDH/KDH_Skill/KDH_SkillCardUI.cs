using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KDH_SkillCardUI : MonoBehaviour
{
    public KDH_SkillData mySkillData;

    public Image iconImage;
    public TextMeshProUGUI nameText;
    // 설명 텍스트를 연결할 변수를 추가합니다!
    public TextMeshProUGUI descText;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (mySkillData != null)
        {
            iconImage.sprite = mySkillData.skillIcon;
            nameText.text = mySkillData.skillName;

            //  데이터의 설명을 UI 텍스트에 넣는 코드를 추가합니다!
            descText.text = mySkillData.skillDescription;
        }
    }
}
using TMPro;
using UnityEngine;

public class KDH_KeySettingName : MonoBehaviour
{
    public TextMeshProUGUI[] txt;
    private void Start()
    {
        RefreshKey();
    }

    public void RefreshKey()
    {
        for (int i = 0; i < txt.Length; i++)
        {
            txt[i].text = KeySetting.keys[(KeyAction)i].ToString(); // 함수를 글자를 바꾸게 설정
        }
    }
}

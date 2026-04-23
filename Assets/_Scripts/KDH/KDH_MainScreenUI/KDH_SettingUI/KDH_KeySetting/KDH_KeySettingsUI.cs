using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { CTRL, SHIFT, Q, E, KEYCOUNT } //열거 형식으로 키 설정

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); //정적인 딕셔너리
}

public class KDH_KeySettingsUI : MonoBehaviour
{
    public KDH_KeySettingName uiText;
    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.LeftControl, KeyCode.LeftShift, KeyCode.Q, KeyCode.E };
    int keyIndex = -1;

    private void Awake()
    {
        //딕셔너리가 비어있을 때만 초기화 (중복 방지)
        if (KeySetting.keys.Count == 0)
        {
            for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
            {
                KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
            }
        }
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;
        // 키 변경 모드(keyIndex >= 0)이고, 실제로 키보드 입력이 들어왔을 때
        if (keyIndex != -1 && keyEvent.isKey && keyEvent.keyCode != KeyCode.None)
        {
            // 딕셔너리 값 변경 (이 순간부터 바뀐 키로만 작동함)
            KeySetting.keys[(KeyAction)keyIndex] = keyEvent.keyCode;

            // UI 텍스트 새로고침 호출
            if (uiText != null) uiText.RefreshKey();

            // 변경 모드 종료
            keyIndex = -1;
        }
    }

    public void ChangeKey(int num)
    {
        keyIndex = num;
        Debug.Log(num + "번 키 변경 중...");
    }
}
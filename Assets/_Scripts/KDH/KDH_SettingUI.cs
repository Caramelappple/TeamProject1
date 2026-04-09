using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    //바텀 시트 부모
    private VisualElement SettingUI;
    //열기 버튼
    private Button SettingButton;
    //닫기 버튼
    private Button CloseButton;


    // Start is called before the first frame update
    void Start()
    {
        //루트 비주얼엘리먼트를 참조한다.
        var root = GetComponent<UIDocument>().rootVisualElement;

        //바텀 시트의 부모
        SettingUI = root.Q<VisualElement>("SettingUI");

        //열기, 닫기 버튼
        SettingButton = root.Q<Button>("SettingButton");
        CloseButton = root.Q<Button>("CloseButton");

        //시작할 때 바텀 시트 그룹을 감춘다.
        SettingUI.style.display = DisplayStyle.None;

        //버튼이 할 일
        SettingButton.RegisterCallback<ClickEvent>(OnOpenButtonClicked);
        CloseButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);
    }

    private void OnOpenButtonClicked(ClickEvent evt)
    {
        //바텀 시트 그룹을 보여준다.
        SettingUI.style.display = DisplayStyle.Flex;
    }

    private void OnCloseButtonClicked(ClickEvent evt)
    {
        //바텀 시트 그룹을 감춘다.
        SettingUI.style.display = DisplayStyle.None;
    }

}
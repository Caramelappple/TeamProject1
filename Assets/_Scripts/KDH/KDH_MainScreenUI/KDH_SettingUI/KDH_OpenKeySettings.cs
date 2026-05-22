using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KDH_OpenKeySettings : MonoBehaviour
{
    [SerializeField] private GameObject _KeySettingUI;
    [SerializeField] private GameObject _CreditUI;
    [SerializeField] private GameObject _MainSceneUI;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void KeySetting ()
    {
        _KeySettingUI.SetActive(true);
        _CreditUI.SetActive(false);
        _MainSceneUI.SetActive(false);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        _CreditUI.SetActive(true);
        _MainSceneUI.SetActive(true);


        Time.timeScale = 1;
    }
}
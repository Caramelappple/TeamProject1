using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KDH_CreditSetting : MonoBehaviour
{
    [SerializeField] private GameObject _KeySettingUI;
    [SerializeField] private GameObject _CreditUI;
    [SerializeField] private GameObject _MainSceneUI;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void CreditSetting()
    {
        _KeySettingUI.SetActive(false );
        _MainSceneUI.SetActive(false );
        _CreditUI.SetActive(true);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        _KeySettingUI.SetActive(true);
        _MainSceneUI.SetActive(true);

        Time.timeScale = 1;
    }
}
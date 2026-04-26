using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KDH_OpenKeySettings : MonoBehaviour
{
    [SerializeField] private GameObject _KeySettingUI;
    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void KeySetting ()
    {
        _KeySettingUI.SetActive(true);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
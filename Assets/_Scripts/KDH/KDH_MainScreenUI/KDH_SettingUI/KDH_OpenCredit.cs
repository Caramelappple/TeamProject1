using UnityEngine;

public class KDH_OpenCredit : MonoBehaviour
{
    [SerializeField] private GameObject _KeySettingUI;
    [SerializeField] private GameObject _Credit;
    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenSetting() // 내 버튼, 내 그라운드 빼고 다 비활성화
    {
        gameObject.SetActive(true);
        _Credit.SetActive(true);
        _KeySettingUI.SetActive(false);
    }

    public void Exit() // 내 버튼, 내 그라운드 다 비활성화
    {
        gameObject.SetActive(false);
        _KeySettingUI.SetActive(true);
        _Credit.SetActive(false);
    }
}

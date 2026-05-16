using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class KDH_KeyManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingMenuUI;
    private InputAction _onSettingMenuAction;

    [SerializeField] private GameObject skillSelectUI;
    [SerializeField] private GameObject skillDeleteUI;

    private void Start()
    {
        _onSettingMenuAction = new InputAction(binding: "<Keyboard>/escape");
        _onSettingMenuAction.performed += OnMainMenu;
        _onSettingMenuAction.Enable();
    }

    private void OnMainMenu(InputAction.CallbackContext context)
    {
        ToggleMenu();
    }
    public void ToggleMenu()
    {
        if (_settingMenuUI.activeSelf)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
    public void OpenMenu()
    {
        if (skillSelectUI.activeSelf || skillDeleteUI.activeSelf) return;

        _settingMenuUI.SetActive(true);
        StartCoroutine(PauseAfterDelay(0.01f));
    }
    
    public void CloseMenu()
    {
        Time.timeScale = 1;
        _settingMenuUI.SetActive(false);
        Debug.Log("게임 재개");
    }

    private IEnumerator PauseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0;
        Debug.Log("일시정지 완료");
    }

    private void OnDestroy()
    {
        if (_onSettingMenuAction != null)
        {
            _onSettingMenuAction.performed -= OnMainMenu;
            _onSettingMenuAction.Disable();
        }
    }
}
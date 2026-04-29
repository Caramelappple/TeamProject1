using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class KDH_KeyManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingMenuUI;
    private InputAction _onSettingMenuAction;
    private bool isMenuOpen;

    private void Start()
    {
        isMenuOpen = false;

        _onSettingMenuAction = new InputAction(binding: "<Keyboard>/escape");
        _onSettingMenuAction.performed += OnMainMenu;
        _onSettingMenuAction.Enable();
    }

    // 1. ESC 키를 눌렀을 때 실행되는 곳
    private void OnMainMenu(InputAction.CallbackContext context)
    {
        // 핵심 로직이 담긴 ToggleMenu 함수를 실행만 시킵니다.
        ToggleMenu();
    }

    // 2. 핵심 로직: UI 버튼에서도 호출할 수 있도록 반드시 'public'으로 선언합니다.
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen; // 상태 반전

        if (isMenuOpen)
        {
            _settingMenuUI.SetActive(true);
            StartCoroutine(PauseAfterDelay(0.01f));
        }
        else
        {
            Time.timeScale = 1;
            _settingMenuUI.SetActive(false);
            Debug.Log("게임 재개");
        }
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
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class KDH_KeyManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingMenuUI; // 세팅메뉴
    private InputAction _onSettingMenuAction; // 세팅메뉴 
    private bool isMenuOpen; // 세팅메뉴를 킬 수 있는지 없는지

    private void Start()
    {
        isMenuOpen = false; // 시작하면 끔

        _onSettingMenuAction = new InputAction(binding: "<Keyboard>/escape"); // ESC키를 누르는 값
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
        yield return new WaitForSecondsRealtime(delay); //소리를 내기 위해 만든 코루틴
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
using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_OpenMainMenuSceneSetting : MonoBehaviour
{
    [SerializeField] private GameObject _KeySettingUI;
    [SerializeField] private GameObject _CreditUI;
    [SerializeField] private GameObject _MainSceneUI;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenMainMenuScene()
    {
        _KeySettingUI.SetActive(false);
        _MainSceneUI.SetActive(true);
        _CreditUI.SetActive(false);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        _KeySettingUI.SetActive(true);
        _CreditUI.SetActive(true);

        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

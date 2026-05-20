using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_MainMenuGo : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Dead()
    {
        gameObject.SetActive(true);
    }

    public void GoMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
        gameObject.SetActive(false);
    }   
}

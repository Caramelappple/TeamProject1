using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_MainMenuGo : MonoBehaviour
{
    private GameObject me;

    private void Start()
    {
        me = gameObject;
        me.SetActive(false);
    }

    public void MakeUI()
    {
        StartCoroutine(Wait());
    }

    public void Dead()
    {
        me.SetActive(true);
    }

    public void GoMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
        me.SetActive(false);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        Dead();
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_MainMenuGo : MonoBehaviour
{
    [SerializeField] private GameObject me;

    private void Start()
    {
        if (me != null) me.SetActive(false);
    }

    public void MakeUI()
    {
        gameObject.SetActive(true);

        StartCoroutine(Wait());
    }

    public void GoMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(4f);

        if (me != null) me.SetActive(true);
    }
}
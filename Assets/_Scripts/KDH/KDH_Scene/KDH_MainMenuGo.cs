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
        StartCoroutine(ResetAndGoMainRoutine());
    }

    private IEnumerator ResetAndGoMainRoutine()
    {
        if (KDH_SceneFader.Instance != null)
        {
            KDH_SceneFader.Instance.FadeToScene("MainMenu");
        }
        yield return new WaitForSeconds(1.0f);

        // 저장된 게임 데이터가 있다면 초기화 (필요 없다면 제외 가능)
        PlayerPrefs.DeleteAll();
        Debug.Log("게임 데이터 초기화 완료");

        // DontDestroyOnLoad에 누적된 다른 오브젝트들을 파괴합니다.
        DestroyAllDontDestroyOnLoadObjects();

        SceneManager.LoadScene("MainMenu");
    }

    private void DestroyAllDontDestroyOnLoadObjects()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent == null && obj.scene.buildIndex == -1)
            {
                Destroy(obj);
            }
        }
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
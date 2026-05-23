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
        if (me != null) me.SetActive(false);
        StartCoroutine(WipeOutAndGoMainRoutine());
    }

    private IEnumerator WipeOutAndGoMainRoutine()
    {
        if (KDH_SceneFader.Instance != null)
        {
            KDH_SceneFader.Instance.FadeToScene("MainMenu");
        }

        yield return new WaitForSeconds(1.0f);

        PlayerPrefs.DeleteAll();
        Debug.Log("모든 로컬 데이터 포맷 완료");

        NullifyAllSingletons();
        WipeOutEverything();

        Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in allCanvases)
        {
            canvas.gameObject.SetActive(false);
        }

        // 메인 메뉴 씬을 새로 로드
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void NullifyAllSingletons()
    {
        LSO_PlayerMovement.instance = null;
        NKY_GameManager.instance = null;
        KDH_HealthBarBossUI.instance = null;
    }

    private void WipeOutEverything()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        Scene currentScene = SceneManager.GetActiveScene();

        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent == null)
            {
                if (obj.scene.buildIndex == -1)
                {
                    if (obj != gameObject && !transform.IsChildOf(obj.transform) && obj != transform.root.gameObject)
                    {
                        Debug.Log($"[UI/오브젝트 소각 이사] : {obj.name}");
                        SceneManager.MoveGameObjectToScene(obj, currentScene);
                    }
                }
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
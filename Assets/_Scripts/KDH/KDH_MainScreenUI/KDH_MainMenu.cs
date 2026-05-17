using UnityEngine;

public class KDH_MainMenu : MonoBehaviour
{
    [SerializeField] private KDH_SceneTransition FadeUi;
    public void OnClickNewGame ()
    {
        Debug.Log("게임 플레이"); //클릭하면 새 게임 생성(아직 구현X)
        FadeUi.GoToNextScene("InGame");
    }

    public void OnClickOption ()
    {
        Debug.Log("옵션"); //클릭하면 옵션창 등장
    }

    public void OnClickQuit ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //클릭하면 게임 종료
#else
        Application.Quit(); //클릭하면 게임 종료
#endif
    }
}
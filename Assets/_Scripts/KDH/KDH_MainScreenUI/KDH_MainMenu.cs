using UnityEngine;

public class KDH_MainMenu : MonoBehaviour
{
    public void OnClickNewGame ()
    {
        Debug.Log("새 게임"); //클릭하면 새 게임 생성(아직 구현X)
    }

    public void OnClickLoad ()
    {
        Debug.Log("불러오기"); //클리하면 이전 게임으로 플레이(구현 X)
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
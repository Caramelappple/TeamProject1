using TMPro;
using UnityEngine;

public class KDH_EndingCreditManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI creditText; // 인스펙터에서 텍스트 UI를 연결해 줄 칸

    // @ 기호를 사용해 긴 텍스트를 그대로 담습니다.
    private string creditContent = @"THE END
















긴 여정이 마침내 끝을 맺었다.
검 끝에 머물던 뜨거운 피도,
고막을 찢을 듯한 굉음도
이제는 아득한 메아리가 되었다.



처음 마주했던 압도적인 절망이 기억난다.
대지를 태우던 화염의 야수,
차가운 강철의 심장을 가졌던 기사.
그리고 하늘을 뒤덮었던 거대한 마수의 그림자까지.



끝이 없을 것 같던 악몽이 마침내 걷혔다.
이제 나는 낡고 피 묻은 무기를
이 땅에 꽂아 둔다.



나의 이야기는 여기까지다.";

    void Start()
    {
        // 게임이 시작될 때 텍스트 UI에 내용을 덮어씌웁니다.
        if (creditText != null)
        {
            creditText.text = creditContent;
        }
    }
}

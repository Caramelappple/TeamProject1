using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KDH_SettingUI : MonoBehaviour
{
    [SerializeField] private Button MouseControlButton; //마우스 설정 버튼
    [SerializeField] private Button KeyboardMouseControlButton; //마우스 + 키보드 설정 버튼
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        switch (KDH_PlayerSettings.controlType)
        {
            case EControlType.Mouse: //마우스이면
                MouseControlButton.image.color = Color.green; //선택한 것을 초록색으로
                KeyboardMouseControlButton.image.color = Color.white; //그 외의 것은 흰색으로
                break;
            case EControlType.KeyboardMouse: //마우스와 키보드이면
                MouseControlButton.image.color = Color.white; //그 외의 것은 흰색으로
                KeyboardMouseControlButton.image.color = Color.green; //선택한 것을 초록색으로
                break;
        }
    }

    public void SetControlMode(int controlType)
    {
        KDH_PlayerSettings.controlType = (EControlType)controlType; //지금 설정된 상태
        switch (KDH_PlayerSettings.controlType)
        {
            case EControlType.Mouse: //위에 있는거랑 똑같은.
                MouseControlButton.image.color = Color.green;
                KeyboardMouseControlButton.image.color = Color.white;
                break;
            case EControlType.KeyboardMouse:
                MouseControlButton.image.color = Color.white;
                KeyboardMouseControlButton.image.color = Color.green;
                break;
        }
    }

    public void Close() //창을 닫는 함수
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay() //애니메이션을 실행하기 위한
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("Close");
    }
}
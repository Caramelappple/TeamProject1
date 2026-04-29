using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KDH_Skill : MonoBehaviour
{
    [SerializeField]
    private string skillName;               // 해당 스킬 이름
    [SerializeField]
    private float maxCooldownTime;      // 해당 스킬 재사용 대기 시간
    [SerializeField]
    private TextMeshProUGUI textSkillData;          // 스킬 시전 정보 출력
    [SerializeField]
    private TextMeshProUGUI textCooldownTime;       // 재사용 대기 시간을 텍스트로 출력하는 Text UI
    [SerializeField]
    private Image imageCooldownTime;        // 재사용 대기 시간을 이미지로 출력하는 Image UI

    private float currentCooldownTime;  // 현재 재사용 대기 시간
    private bool isCooldown;             // 현재 쿨타임이 적용중인지 체크

    private void Awake()
    {
        SetCooldownIs(false); // 시작전 쿨타임 상태를 끔
    }

    /// <summary>
    /// 외부에서 스킬을 사용할 때 호출하는 메소드
    /// </summary>
    
    public void UseSkill()
    {
        // 이미 스킬을 사용해서 재사용 대기 시간이 남아있으면 종료
        if (isCooldown == true) // 쿨다운이 가능하다면?
        {
            textSkillData.text = $"[{skillName}] Cooldown Time : {currentCooldownTime:F1}"; // 쿨타임 남은 값을 UI 텍스처로 나타냄
            return;
        }

        textSkillData.text = $"Use Skill : {skillName}"; // 사용한 스킬의 이름을 표시

        StartCoroutine(nameof(OnCooldownTime), maxCooldownTime); // OnCooldownTime을 문자열형태로 가져와서 코루틴 실행. (nameof의 장점은 오타 방지를 막아서 오류를 잡아줌)
    }

    /// <summary>
    /// 실제 스킬의 재사용 대기 시간을 제어하는 코루틴 메소드
    /// </summary>
    private IEnumerator OnCooldownTime(float maxCooldownTime)
    {
        // 스킬 재사용 대기 시간 저장
        currentCooldownTime = maxCooldownTime;

        SetCooldownIs(true);

        while (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
            // Image UI의 fiilAmount를 조절해 채워지는 이미지 모양 설정
            imageCooldownTime.fillAmount = currentCooldownTime / maxCooldownTime;
            // Text UI에 쿨다운 시간 표시
            textCooldownTime.text = currentCooldownTime.ToString("F1");

            yield return null; //다음 프레임이 될 때까지 기다림.
        }

        SetCooldownIs(false); //끝나면 쿨다운 상태를 false로 함
    }

    private void SetCooldownIs(bool boolean)
    {
        isCooldown = boolean; // 쿨다운이 가능한가?
        textCooldownTime.enabled = boolean; // text의 쿨타임이 가능한가?
        imageCooldownTime.enabled = boolean; // 이미지의 쿨타임이 가능한가?
    }
}

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
    private TextMeshProUGUI textSkillData; // 해당 스킬 데이터를 UI로 출력
    [SerializeField]
    private TextMeshProUGUI textCooldownTime;       // 재사용 대기 시간을 텍스트로 출력하는 Text UI
    [SerializeField]
    private Image imageCooldownTime;        // 재사용 대기 시간을 이미지로 출력하는 Image UI

    public GameObject skills;               // 실제로 사용될 스킬들

    private float currentCooldownTime;  // 현재 재사용 대기 시간
    private bool isCooldown;             // 현재 쿨타임이 적용중인지 체크

    private void Awake()
    {
        SetCooldownIs(false);
    }

    /// <summary>
    /// 외부에서 스킬을 사용할 때 호출하는 메소드
    /// </summary>
    public void UseSkill()
    {
        if (textSkillData == null) return;

        if (isCooldown == true)
        {
            textSkillData.text = $"[{skillName}] Cooldown Time : {currentCooldownTime:F1}";
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");

        Vector3 spawnPos = transform.position;
        Quaternion spawnRot = transform.rotation;

        if (player != null)
        {
            // 플레이어가 있다면 플레이어 위치로 설정
            spawnPos = player.transform.position;
            spawnRot = player.transform.rotation;
        }

        // 설정된 위치에 스킬 소환
        if (skills != null)
        {
            Instantiate(skills, spawnPos, spawnRot);
            Debug.Log($"{skillName} 소환");
        }

        textSkillData.text = $"Use Skill : {skillName}";
        StartCoroutine(nameof(OnCooldownTime), maxCooldownTime);
    }
    public void SetTextReference(TextMeshProUGUI sceneText)
    {
        textSkillData = sceneText;
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

            yield return null;
        }

        SetCooldownIs(false);
    }

    private void SetCooldownIs(bool boolean)
    {
        isCooldown = boolean;
        textCooldownTime.enabled = boolean;
        imageCooldownTime.enabled = boolean;
    }
}

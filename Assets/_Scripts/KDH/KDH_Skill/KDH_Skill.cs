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

    //private LSO_Ice iceSkill = new LSO_Ice();

    private float _currentCooldownTime;  // 현재 재사용 대기 시간
    private bool _isCooldown;     
    
    private GameObject _skillObj;
    // 현재 쿨타임이 적용중인지 체크

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

        if (_isCooldown)
        {
            textSkillData.text = $"[{skillName}] Cooldown Time : {_currentCooldownTime:F1}";
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) { Debug.LogError("Player 태그를 찾을 수 없습니다!"); return; }

        // 1. 프리팹 자체가 할당되어 있는지 확인
        if (skills == null)
        {
            Debug.LogError($"{skillName}의 skillPrefab이 비어있습니다!");
            return;
        }


        if (!_skillObj)
        {
            _skillObj = Instantiate(skills, player.transform.position, player.transform.rotation);
            _skillObj.gameObject.transform.SetParent(gameObject.transform);//이시온이 고쳐야 함
        }

        // 2. 스크립트(인터페이스)가 들어있는지 확인
        LSO_ISkill skillLogic = _skillObj.GetComponent<LSO_ISkill>();

        if (skillLogic != null)
        {
            skillLogic.UseSkill(player);
            Debug.Log($"{skillName} 실행 완료");

            textSkillData.text = $"Use Skill : {skillName}";
            StartCoroutine(nameof(OnCooldownTime), maxCooldownTime);
        }
        else
        {
            // 이 메시지가 뜬다면 프리팹에 스크립트를 안 붙인 것입니다.
            Debug.LogError($"{skills.name} 프리팹에 LSO_ISkill 컴포넌트가 없습니다!");
            Destroy(_skillObj); // 실행 불가능한 오브젝트는 지워줍니다.
        }
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
        _currentCooldownTime = maxCooldownTime;

        SetCooldownIs(true);

        while (_currentCooldownTime > 0)
        {
            _currentCooldownTime -= Time.deltaTime;
            // Image UI의 fillAmount를 조절해 채워지는 이미지 모양 설정
            imageCooldownTime.fillAmount = _currentCooldownTime / maxCooldownTime;
            // Text UI에 쿨다운 시간 표시
            textCooldownTime.text = _currentCooldownTime.ToString("F1");

            yield return null;
        }

        SetCooldownIs(false);
    }

    private void SetCooldownIs(bool boolean)
    {
        _isCooldown = boolean;
        textCooldownTime.enabled = boolean;
        imageCooldownTime.enabled = boolean;
    }
}

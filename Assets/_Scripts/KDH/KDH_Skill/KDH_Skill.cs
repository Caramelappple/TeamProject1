using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KDH_Skill : MonoBehaviour
{
    [SerializeField]
    private string skillName;               // 해당 스킬 이름
    [SerializeField]
    private float maxCooldownTime;          // 해당 스킬 재사용 대기 시간
    [SerializeField]
    private TextMeshProUGUI textSkillData;  // 해당 스킬 데이터를 UI로 출력
    [SerializeField]
    private TextMeshProUGUI textCooldownTime;       // 재사용 대기 시간을 텍스트로 출력하는 Text UI
    [SerializeField]
    private Image imageCooldownTime;        // 재사용 대기 시간을 이미지로 출력하는 Image UI

    public GameObject skills;               // 실제로 사용될 스킬들

    private float _currentCooldownTime;     // 현재 재사용 대기 시간
    private bool _isCooldown;

    private GameObject _skillObj;

    private void Awake()
    {
        SetCooldownIs(false);
    }

    public GameObject SkillPrefab
    {
        get { return skills; }
        set { skills = value; }
    }

    public string SkillName
    {
        get { return skillName; }
    }

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

        if (skills == null)
        {
            Debug.LogError($"{skillName}의 skillPrefab이 비어있습니다!");
            return;
        }

        if (!_skillObj)
        {
            _skillObj = Instantiate(skills, player.transform.position, player.transform.rotation);
            _skillObj.gameObject.transform.SetParent(gameObject.transform);
        }

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
            Debug.LogError($"{skills.name} 프리팹에 LSO_ISkill 컴포넌트가 없습니다!");
            Destroy(_skillObj);
        }
    }

    public void SetTextReference(TextMeshProUGUI sceneText)
    {
        textSkillData = sceneText;
    }

    private IEnumerator OnCooldownTime(float maxCooldownTime)
    {
        _currentCooldownTime = maxCooldownTime;
        SetCooldownIs(true);

        while (_currentCooldownTime > 0)
        {
            _currentCooldownTime -= Time.deltaTime;
            imageCooldownTime.fillAmount = _currentCooldownTime / maxCooldownTime;
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
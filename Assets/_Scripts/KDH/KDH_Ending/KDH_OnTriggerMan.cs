using UnityEngine;

public class KDH_OnTriggerMan : MonoBehaviour
{
    [SerializeField] private GameObject endingBox;
    private KDH_SkillSystem _skillSystem;
    private void Start()
    {
        _skillSystem = FindFirstObjectByType<KDH_SkillSystem>();
        endingBox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(endingBox);

        if (collision.CompareTag("Player"))
        {
            endingBox.SetActive(true);
            NKY_SoundManager.Instance.StopBGM();

            // 1. 스킬 시스템 비활성화
            if (_skillSystem != null)
            {
                _skillSystem.SetCanUseSkill(false);
            }

            // 2. 다리를 잘라서 못 움직기게 하기
            LSO_PlayerMovement.instance._rigid.simulated = false;

            // 3. 평타(공격) 시스템 비활성화
            if (collision.gameObject.TryGetComponent<LSO_PlayerAttack>(out LSO_PlayerAttack playerAttack))
            {
                playerAttack.SetCanAttack(false);
            }
        }
    }
}

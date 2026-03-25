using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("설정")]
    public string playerTag = "Player"; // 플레이어 오브젝트의 태그
    public float moveSpeed = 3.0f;      // 이동 속도
    public float stopDistance = 1.0f;   // 플레이어와 유지할 최소 거리

    private Transform playerTransform;

    void Start()
    {
        // 씬 시작 시 "Player" 태그를 가진 오브젝트를 찾습니다.
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("씬에 'Player' 태그를 가진 오브젝트가 없습니다!");
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // 플레이어와 적 사이의 거리 계산
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // 일정 거리 이상일 때만 추적 (너무 딱 붙지 않게)
        if (distance > stopDistance)
        {
            // 현재 위치에서 플레이어 위치로 이동
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerTransform.position,
                moveSpeed * Time.deltaTime
            );

            // (선택 사항) 적이 이동 방향을 바라보게 하고 싶다면?
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        // 2D 탑뷰에서 스프라이트 회전 (필요 없다면 삭제 가능)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // 90도 조정은 스프라이트 방향에 따라 가감
    }
}
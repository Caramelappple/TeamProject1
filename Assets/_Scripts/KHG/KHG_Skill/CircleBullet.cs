using UnityEngine;

public class CircleBullet : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public float speed = 7f;

    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        FindTarget(); // 시작할 때 타겟 찾기
    }

    void FindTarget()
    {
        GameObject enemyObj = GameObject.FindWithTag(enemyTag);
        if (enemyObj != null)
        {
            target = enemyObj.transform;
        }
    }

    void FixedUpdate()
    {
        // 만약 타겟이 중간에 사라졌거나 처음에 못 찾았다면 다시 검색
        if (target == null)
        {
            FindTarget();

            // 여전히 적이 없다면 파괴 (또는 그냥 직진)
            if (target == null)
            {
                // rb.linearVelocity = transform.right * speed; // 적 없으면 직진하려면 주석 해제
                return;
            }
        }

        // 타겟 방향으로 이동
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        // 스프라이트 방향 반전
        if (rb.linearVelocity.x != 0)
        {
            spriteRenderer.flipX = (rb.linearVelocity.x < 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            Destroy(gameObject);
        }
    }
}
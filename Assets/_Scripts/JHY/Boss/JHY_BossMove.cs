using UnityEngine;

public class JHY_BossMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;
    private Vector3 baseScale;
    //move
    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    private SpriteRenderer sr;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float stopRange = 2f;
    private bool isArrived = false;
    public bool isMoving;

    void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }

    void Update()
    {
        if (player == null) return;
        LookAtPlayer();

        if (!isArrived)
        {
            Vector2 targetPos = new Vector2(3.56f, -0.83f);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            float distance = Vector2.Distance(transform.position, targetPos);
            ani.SetFloat("Speed", speed);
            isMoving = true;
            if (distance < 0.01f)
            {
                isArrived = true;
                isMoving = false;
                ani.SetFloat("Speed", 0);
            }
        }
        else
        {
            float distance2 = Vector2.Distance(transform.position, player.position);

            if (distance2 > chaseRange)
            {
                MoveTowardsPlayer();
                isMoving = true;
            }
            else if (distance2 <= stopRange)
            {
                StopMoving();
            }
            else
            {
                ani.SetFloat("Speed", 0);
            }
        }
    }

    void LookAtPlayer()
    {
        // 플레이어가 보스보다 오른쪽에 있을 때
        if (player.position.x > transform.position.x)
        {
            // 원래 방향 (오른쪽)
            transform.localScale = new Vector3(-baseScale.x, baseScale.y, baseScale.z);
        }
        // 플레이어가 보스보다 왼쪽에 있을 때
        else
        {
            // X축 값을 -1로 만들어 전체를 반전 (왼쪽)
            transform.localScale = new Vector3(baseScale.x, baseScale.y, baseScale.z);
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 targetPos = new Vector2(player.position.x, player.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        ani.SetFloat("Speed", speed);
        isMoving = true;
    }

    public void StopMoving()
    {
        ani.SetFloat("Speed", 0f);
        isMoving = false;
    }
}
using UnityEngine;

public class BossFollow2D : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float followRange = 10f;
    public float keepDistance = 3f;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        float currentSpeed = 0f;

        if (distance <= followRange && distance > keepDistance)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.position = (Vector2)transform.position + dir * moveSpeed * Time.deltaTime;
            currentSpeed = moveSpeed;

            if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }

        anim.SetFloat("Speed", currentSpeed);
    }
}
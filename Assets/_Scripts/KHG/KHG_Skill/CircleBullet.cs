using System.Collections;
using UnityEngine;

public class KHG_CircleBullet : MonoBehaviour,LSO_ISkill
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
        FindTarget();
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
        if (target == null)
        {
            FindTarget();

            if (target == null)
            {
                return;
            }
        }

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

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

    public void UseSkill(GameObject player)
    {

    }

    public IEnumerator CoolTime(float time)
    {
        throw new System.NotImplementedException();
    }
}
using System.Collections;
using UnityEngine;

public class CircleBullet : MonoBehaviour,LSO_ISkill
{
    private readonly string _enemyTag = "Enemy";
    public float speed = 7f;

    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    bool hasTarget
    {
        get { return target != null; }
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        FindTarget(); 
    }

    private void FindTarget()
    {
        GameObject enemyObj = GameObject.FindWithTag(_enemyTag);
        if (enemyObj != null)
            target = enemyObj.transform;
    }

    private void FixedUpdate()
    {
        if(!hasTarget) return;
        Move();
        FlipX();
    }

    private void Move()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void FlipX()
    {
        if (rb.linearVelocity.x != 0)
            spriteRenderer.flipX = (rb.linearVelocity.x < 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
            Destroy(gameObject);
    }

    public void UseSkill(GameObject player)
    {
    }

    public IEnumerator CoolTime(float time)
    {
        yield break; 
    }
}
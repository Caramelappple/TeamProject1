using UnityEngine;

public class JHY_Boss2Move : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private Transform player;
    private SpriteRenderer sr;
    private Animator ani;
    private Rigidbody2D rb;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }
    private void Update()
    {
        Look();
    }
    void Look()
    {
        if (player.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
    }
}

using UnityEngine;

public class JHY_Spire : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 투사체 속도
    [SerializeField] private float destroyTime = 3f; // 몇 초 뒤에 사라질지
    private Rigidbody2D rb;

    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed; // transform.right(빨간 화살표 방향)로 발사

        Destroy(gameObject, destroyTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

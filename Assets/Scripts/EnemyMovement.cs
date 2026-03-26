using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rigid;
    [SerializeField] private Rigidbody2D playerRigid;
    private float speed = 3f;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rigid.linearVelocity = (playerRigid.position - rigid.position).normalized * speed;
    }
}

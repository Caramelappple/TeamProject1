using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LSO_PlayerMovement : MonoBehaviour
{
    protected Rigidbody2D playerRigid;
    [SerializeField]protected float speed = 10f;
    private Vector2 dir;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        playerRigid.linearVelocity = dir.normalized * speed;
        transform.Rotate(Vector3.forward * speed);
    }
    
    private void OnMove(InputValue value)
    {
        dir = value.Get<Vector2>();
    }
}

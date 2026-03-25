using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoving : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float speed = 10;
    private float rotatespeed = 300;
    private Vector2 dir;
    [SerializeField] private GameObject firePos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        rigid.linearVelocity = dir.normalized * speed;
        firePos.transform.Rotate(Vector3.forward * rotatespeed * Time.deltaTime);
    }
    private void OnMove(InputValue value)
    {
        dir = value.Get<Vector2>();
    }
}

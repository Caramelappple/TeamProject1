using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigid;
    private float _speed = 10.0f;
    private Vector2 _moveDir;

    public void Awake()
    {
        //리지드 바디 컴포넌트 가져오기
        _rigid.GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        //플레이어 움직임 정도
        _rigid.linearVelocity = (Vector3)_moveDir * _speed; 
    }

    public void OnMove (InputValue value)
    {
        //방향을 입력
        _moveDir = value.Get<Vector2>();
    }
}

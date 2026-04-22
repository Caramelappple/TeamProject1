using UnityEngine;
using UnityEngine.InputSystem;

namespace NKY.Player
{
    public class NKY_Player : MonoBehaviour
    {
        public int speed = 5;
        private Vector2 _moveDir;
        private Rigidbody2D _rb;
        //[SerializeField] private float _jumpPower = 5;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            transform.position += (Vector3)_moveDir * speed * Time.deltaTime;
        }
        void OnMove(InputValue inputValue)
        {
            _moveDir = inputValue.Get<Vector2>();
        }
        void OnJump()
        {
            Debug.Log("jump");
        }

    }
}

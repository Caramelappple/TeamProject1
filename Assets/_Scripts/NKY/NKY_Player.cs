using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.NKY
{
    public class NKY_Player : MonoBehaviour
    {
        public GameObject target;
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
            transform.position += (Vector3)_moveDir * (speed * Time.deltaTime);
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                if (target.TryGetComponent<NKY_Health>(out var health))
                {
                    health.GetDamage(NKY_DamageData.Create(health, 20));
                }
            }
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

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
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                DamageData data = DamageData.Create(this.GetComponent<Health>(), 20);
                if (target.TryGetComponent<Health>(out var health))
                {
                    health.GetDamage(data);
                }
            }
        }

        void OnMove(InputValue inputValue)
        {
            _moveDir = inputValue.Get<Vector2>();
            _rb.linearVelocity = _moveDir * speed;
        }
    }
}

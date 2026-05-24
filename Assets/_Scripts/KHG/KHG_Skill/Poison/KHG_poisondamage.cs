using System.Collections.Generic;
using UnityEngine;

public class KHG_PoisonDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float interval = 0.01f;

    private Health _playerHealth;
    
    private Animator _animator;

    public void Init(Health playerHealth)
    {
        _playerHealth = playerHealth;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

        private Dictionary<GameObject, float> _timer = new Dictionary<GameObject, float>();

   
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        
        if (!_playerHealth ) return;
        _timer.TryAdd(collision.gameObject, 0f);

        _timer[collision.gameObject] += Time.deltaTime;

        if (_timer[collision.gameObject] >= interval)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health health))
            {
                DamageData data = new DamageData(_playerHealth, damage);
                health.GetDamage(data);
            }

            _timer[collision.gameObject] = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _timer.Remove(other.gameObject);
    }
    
    private void FixedUpdate()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Animation-magic-12-e") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
    }
}

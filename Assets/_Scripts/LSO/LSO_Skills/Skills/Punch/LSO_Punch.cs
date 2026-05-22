using System;
using UnityEngine;

public class LSO_Punch : MonoBehaviour
{
   private int _damage = 2;
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

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (!_playerHealth) return;
      if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health health))
      {
         DamageData data = new DamageData(_playerHealth, _damage);
         health.GetDamage(data);
      }
   }
   
   private void FixedUpdate()
   {
      if (!_animator || !gameObject) return;
        
      AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
      if (stateInfo.IsName("5-c") && stateInfo.normalizedTime >= 0.95f)
      {
         gameObject.SetActive(false);
      }
   }
}

using System.Collections;
using UnityEngine;

public class LSO_IceSpike : MonoBehaviour
{
    private int _damage = 10;
    private float _time = 1.8f;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
        {
            //맞은 적에게 데미지
            DamageData data = DamageData.Create(enemyHealth, _damage);
            enemyHealth.GetDamage(data);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(IceSpike());
    }

    IEnumerator IceSpike()
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}

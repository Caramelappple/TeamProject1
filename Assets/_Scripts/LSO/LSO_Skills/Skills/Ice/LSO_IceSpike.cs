using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LSO_IceSpike : MonoBehaviour
{
    private int _damage = 10;
    private float _time = 1.8f;
    private void OnEnable()
    {
        StartCoroutine(_IceSpike());
    }

    IEnumerator _IceSpike()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale/2, 0);
        foreach (Collider2D collision in colliders)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                //맞은 적에게 데미지
                DamageData data = DamageData.Create(enemyHealth, _damage);
                enemyHealth.GetDamage(data);
            }
        }
        yield return  new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class LSO_Lightning : MonoBehaviour
{
    private int _damage = 8;
    private float _time = 2.4f;
    
    private void OnEnable()
    {
        StartCoroutine(Lightning());
    }

    IEnumerator Lightning()
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
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}

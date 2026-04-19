using UnityEngine;

public class KDH_EnemyTest : MonoBehaviour
{
    private KDH_Entity _entity;
    private int _damageAmount = 10;
    private KDH_Health _myHealth;

    private void Awake()
    {
        _myHealth = GetComponent<KDH_Health>();
        _entity = GetComponent<KDH_Entity>();

        if (_entity != null)
        {
            _entity.enemy = gameObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_myHealth != null && _myHealth._isDestroyed) return;

        if (collision.collider.CompareTag("Player"))
        {
            KDH_Health playerHealth = collision.gameObject.GetComponent<KDH_Health>();

            if (playerHealth != null)
            {
                playerHealth.GetDamage(_damageAmount, _entity);
                Debug.Log($"남은 플레이어의 체력{playerHealth.Value}");
            }
        }
    }
}
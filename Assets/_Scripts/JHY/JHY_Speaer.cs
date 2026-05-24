using UnityEngine;

public class JHY_BossProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 6;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.right * (speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Health playerHealth = other.GetComponent<Health>();
        if (playerHealth != null)
        {
            DamageData data = DamageData.Create(null, damage);
            playerHealth.GetDamage(data);
        }

    }


}

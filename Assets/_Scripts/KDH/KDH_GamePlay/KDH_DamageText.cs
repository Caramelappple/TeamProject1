using UnityEngine;
using _Scripts.HealthSystem;

public class KDH_DamageText : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;

    private DamageableResources myHealth;

    private void Awake()
    {
        myHealth = GetComponent<DamageableResources>();

        if (myHealth != null)
        {
            myHealth.OnDamage += SpawnText;
        }
    }

    private void SpawnText(DamageResultData resultData)
    {
        int damage = resultData.damage;

        GameObject textObj = Instantiate(damageTextPrefab, transform.position + Vector3.up, Quaternion.identity);
        textObj.GetComponent<KDH_DamageAnim>().Setup(damage);
    }
}
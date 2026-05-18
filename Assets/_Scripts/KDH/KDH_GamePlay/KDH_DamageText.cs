using UnityEngine;

public class KDH_DamageText : DamageableResources
{
    [SerializeField] private GameObject damageTextPrefab;

    public override void GetDamage(DamageData data)
    {
        base.GetDamage(data);

        int damage = data.damage;
        GameObject textObj = Instantiate(damageTextPrefab, transform.position + Vector3.up, Quaternion.identity);
        textObj.GetComponent<KDH_DamageAnim>().Setup(damage);
    }
}

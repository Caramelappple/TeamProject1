using _Scripts.HealthSystem;
using UnityEngine;

public class KDH_HealText : Health
{
    [SerializeField] private GameObject damageTextPrefab;

    public override void Recover(RecoverData data)
    {
        base.Recover(data);

        int recoverValue = data.recoverValue;
        GameObject textObj = Instantiate(damageTextPrefab, transform.position + Vector3.up, Quaternion.identity);
        textObj.GetComponent<KDH_HealAnim>().Setup(recoverValue);
    }
}
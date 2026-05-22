using UnityEngine;
using _Scripts.HealthSystem;

public class KDH_HealText : MonoBehaviour
{
    [SerializeField] private GameObject healTextPrefab;

    private Health myHealth;

    private void Awake()
    {
        myHealth = GetComponent<Health>();

        if (myHealth != null)
        {
            myHealth.OnRecover += SpawnText;
        }
    }

    private void SpawnText(RecoverResultData resultData)
    {
        int heal = resultData.recoverValue;

        GameObject textObj = Instantiate(healTextPrefab, transform.position + Vector3.up, Quaternion.identity);
        textObj.GetComponent<KDH_HealAnim>().Setup(heal);
    }
}
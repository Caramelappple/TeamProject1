using UnityEngine;

public class KDH_InitItemPortal : MonoBehaviour
{
    private Health health;
    [SerializeField] private GameObject itemPack;

    private void Start()
    {
        health = GetComponent<Health>();
    }
    private void Update()
    {
        if (health == null) return;

        if (health.IsDestroyed || health.Value <= 0)
        {
            itemPack.SetActive(true);

            enabled = false;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class KDH_InitItemPortal : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void Enable()
    {
        if (health.Value <= 0)
        {
            Debug.Log("죽음");
        }
    }
}

using System;
using UnityEngine;

public class LSO_TempEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) return;
        Destroy(gameObject , 0.5f);
    }
}

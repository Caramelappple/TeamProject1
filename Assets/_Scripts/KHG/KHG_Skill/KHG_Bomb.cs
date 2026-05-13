using System;
using UnityEngine;

public class KHG_Bomb : MonoBehaviour
{
    public GameObject bullet;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(bullet, transform);
        }
    }

}

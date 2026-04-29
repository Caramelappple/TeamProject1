using System;
using System.Collections;
using UnityEngine;

public class KHG_Item : MonoBehaviour
{

    private bool isStarted = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isStarted) return;
        StartCoroutine(Spawn());   
    }

    private IEnumerator Spawn()
    {
        isStarted = false;
        Debug.Log("1");
        yield return new WaitForSeconds(5f);
        Debug.Log("2");
        isStarted = true;
        Destroy(gameObject);
    }

}

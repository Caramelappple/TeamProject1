using System.Collections;
using UnityEngine;

public class KHG_ItemSpawner : MonoBehaviour
{
    private float time;
    [SerializeField] private GameObject item;
    private void Start()
    {
        time = Random.Range(20f, 61f);
        StartCoroutine(test());
    }
   
    
    IEnumerator test()
    {
        yield return  new WaitForSeconds(time);
        
    }

    //소환할 위치 정하기
    //
}

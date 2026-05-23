using System.Collections;
using UnityEngine;

public class KHG_ItemSpawner : MonoBehaviour
{
    private float _time;
    [SerializeField] private GameObject item;
    private void Start()
    {
        _time = Random.Range(20f, 61f);
        StartCoroutine(itemspawn());
    }
   
    
    IEnumerator itemspawn()
    {
        yield return  new WaitForSeconds(_time);
        
    }
    

}

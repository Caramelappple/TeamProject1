using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class KHG_ItemSpawn : MonoBehaviour
{
    private bool spawnable = true;
    [SerializeField] private GameObject[] locations;
    private GameObject spawnlocation;
    private GameObject item;
    private GameObject dupeditem;
    private float cooltime = 5f;

    private void FixedUpdate()
    {
        StartCoroutine(Spawn(locations));
    }

    IEnumerator Spawn(GameObject[] locations)
    {
        if (spawnable)
        {
            spawnable = false;
            SpawnItem(locations);
            cooltime = Random.Range(1f,3f);
            yield return new WaitForSeconds(cooltime);
            spawnable = true;
        }
    }
    public string SpawnItem(GameObject[] locations)
    {
        spawnlocation = locations[Random.Range(0, locations.Length)];
        dupeditem = Instantiate(item, spawnlocation.transform.position, Quaternion.identity);
        return dupeditem.name;
    }
    
}

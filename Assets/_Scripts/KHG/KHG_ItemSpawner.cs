using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 2f;

    [SerializeField] private GameObject boss;

    private bool canSpawn = true;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (canSpawn)
        {
            float randomTime = Random.Range(minSpawnTime, maxSpawnTime);

            yield return new WaitForSeconds(randomTime);

            if (boss == null)
            {
                canSpawn = false;
                yield break;
            }

            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(itemPrefab,
            spawnPoints[randomIndex].position,
            Quaternion.identity);
    }
}
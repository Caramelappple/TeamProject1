using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float minSpawnTime = 1f;

    [SerializeField] private float maxSpawnTime = 2f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float randomTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(randomTime);

            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(itemPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }
}
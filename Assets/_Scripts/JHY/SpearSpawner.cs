using System.Collections;
using UnityEngine;

public class SpearSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private float topOffset = 1f;

    public void SpawnSpears()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        Camera cam = Camera.main;
        Vector3 leftTop = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
        Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        for (int i = 0; i < spawnCount; i++)
        {
            float randomX = Random.Range(leftTop.x, rightTop.x);
            Vector3 spawnPos = new Vector3(randomX, leftTop.y + topOffset, 0f);

            Instantiate(spearPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}

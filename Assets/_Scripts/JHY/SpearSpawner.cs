using System.Collections;
using UnityEngine;

public class SpearSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private GameObject warningCirclePrefab;
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private float warningTime = 0.8f;
    [SerializeField] private float topOffset = 1f;
    [SerializeField] private float targetY = -2f;

    public void SpawnSpears()
    {
        StartCoroutine(SpawnSpearsRoutine());
    }

    private IEnumerator SpawnSpearsRoutine()
    {
        Camera cam = Camera.main;
        Vector3 leftTop = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
        Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        for (int i = 0; i < spawnCount; i++)
        {
            float targetX = Random.Range(leftTop.x, rightTop.x);
            Vector3 targetPos = new Vector3(targetX, targetY, 0f);

            StartCoroutine(SpawnOneSpear(targetPos, leftTop.y + topOffset));

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator SpawnOneSpear(Vector3 targetPos, float spawnY)
    {
        GameObject warning = null;

        if (warningCirclePrefab != null)
        {
            warning = Instantiate(warningCirclePrefab, targetPos, Quaternion.identity);
        }

        yield return new WaitForSeconds(warningTime);

        Vector3 spawnPos = new Vector3(targetPos.x, spawnY, 0f);
        Instantiate(spearPrefab, spawnPos, Quaternion.identity);

        if (warning != null)
        {
            Destroy(warning);
        }
    }
}

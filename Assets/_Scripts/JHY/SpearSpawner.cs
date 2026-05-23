using System.Collections;
using UnityEngine;

public class SpearSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private GameObject warningCirclePrefab;
    [SerializeField] private int spawnCount = 20;
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private float warningTime = 0.8f;
    [SerializeField] private float spawnHeightOffset = 3f;
    [SerializeField] private Vector2 warningOffset = new Vector2(0f, -0.8f);
    public void SpawnSpears()
    {
        StartCoroutine(SpawnSpearsRoutine());
    }

    private IEnumerator SpawnSpearsRoutine()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // ★ 루프마다 카메라 위치 새로 계산
            Camera cam = Camera.main;
            Vector3 leftBottom = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
            Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

            float targetX = Random.Range(leftBottom.x, rightTop.x);
            float targetY = Random.Range(leftBottom.y, rightTop.y);
            Vector2 targetPos = new Vector2(targetX, targetY);

            StartCoroutine(SpawnOneSpear(targetPos));
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator SpawnOneSpear(Vector2 targetPos)
    {
        GameObject warning = null;

        if (warningCirclePrefab != null)
        {
            warning = Instantiate(warningCirclePrefab, targetPos + warningOffset, Quaternion.identity);
        }

        yield return new WaitForSeconds(warningTime);

        Vector2 spawnPos = new Vector2(targetPos.x, targetPos.y + spawnHeightOffset);

        GameObject spearObj = Instantiate(spearPrefab, spawnPos, Quaternion.identity);
        FallingSpear spear = spearObj.GetComponent<FallingSpear>();

        if (spear != null)
        {
            spear.SetTarget(targetPos);
        }

        if (warning != null)
        {
            Destroy(warning);
        }
    }
}

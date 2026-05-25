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

    [Header("맵 생성 범위 설정")]
 
    [SerializeField] private float minX = -6.5f;
    [SerializeField] private float maxX = 21.4f;
    [SerializeField] private float minY = -11f;
    [SerializeField] private float maxY = 8f;

    public void SpawnSpears()
    {
        StartCoroutine(SpawnSpearsRoutine());
    }

    private IEnumerator SpawnSpearsRoutine()
    {
        for (int i = 0; i < spawnCount; i++)
        {
           
            float targetX = Random.Range(minX, maxX);
            float targetY = Random.Range(minY, maxY);
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
            NKY_SoundManager.Instance.PlaySFX("FallSpear");
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

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 topLeft = new Vector3(minX, maxY, 0);
        Vector3 topRight = new Vector3(maxX, maxY, 0);
        Vector3 bottomLeft = new Vector3(minX, minY, 0);
        Vector3 bottomRight = new Vector3(maxX, minY, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}

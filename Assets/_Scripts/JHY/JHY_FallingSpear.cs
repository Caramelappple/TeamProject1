using UnityEngine;

public class FallingSpear : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 8f;
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private float maxDistance = 7f;
    [SerializeField] private GameObject hitEffectPrefab;

    private Vector3 startPos;
    private float moveDistance;

    private void Start()
    {
        startPos = transform.position;
        moveDistance = Random.Range(minDistance, maxDistance);
    }

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (Vector2.Distance(startPos, transform.position) >= moveDistance)
        {
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}

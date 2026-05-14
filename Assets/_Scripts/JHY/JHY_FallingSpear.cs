using UnityEngine;

public class FallingSpear : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 8f;
    [SerializeField] private float randomXOffset = 0.3f;
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private float maxDistance = 7f;
    [SerializeField] private float destroyDelay = 3f;

    private Vector2 moveDir;
    private Vector3 startPos;
    private float moveDistance;
    private bool hasStopped;

    private void Start()
    {
        startPos = transform.position;

        float xOffset = Random.Range(-randomXOffset, randomXOffset);
        moveDir = new Vector2(xOffset, -1f).normalized;

        moveDistance = Random.Range(minDistance, maxDistance);
    }

    private void Update()
    {
        if (hasStopped) return;

        transform.position += (Vector3)(moveDir * fallSpeed * Time.deltaTime);

        if (Vector2.Distance(startPos, transform.position) >= moveDistance)
        {
            hasStopped = true;
            Invoke(nameof(DestroySelf), destroyDelay);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}

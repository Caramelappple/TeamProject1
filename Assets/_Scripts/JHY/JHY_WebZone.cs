using UnityEngine;

public class SpiderWebZone : MonoBehaviour
{
    
    [SerializeField] private float webRadius = 3f;          // 거미줄 범위
    private float originalDrag = -1f;
    private Rigidbody2D playerRb;

    private void Update()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, webRadius);
        bool found = false;

        foreach (Collider2D target in targets)
        {
            if (!target.CompareTag("Player")) continue;

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb == null) continue;

            // 처음 진입할 때만 저장
            if (originalDrag < 0f)
            {
                originalDrag = rb.linearDamping;
                playerRb = rb;
            }

            rb.linearDamping = 10f;
            found = true;
        }

        // 범위 밖 나가면 복원
        if (!found && originalDrag >= 0f)
        {
            playerRb.linearDamping = originalDrag;
            originalDrag = -1f;    // 초기화
            playerRb = null;
        }

    }
    private void OnDestroy()
    {
        if (playerRb != null && originalDrag >= 0f)
        {
            playerRb.linearDamping = originalDrag;
        }
    }
}
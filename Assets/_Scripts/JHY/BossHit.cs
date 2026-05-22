using UnityEngine;

public class BossHit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CameraShake.instance.Shake(0.15f, 0.12f);
        }
    }
}

using System.Collections;
using UnityEngine;

public class KHG_PoisonField : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private GameObject poisonPrefab;

    [SerializeField] private float spawnDistance = 1.5f;
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float moveTime = 0.3f;

    [SerializeField] private float lifeTime = 5f;

    private float _coolTime = 3f;
    private bool _canUse = true;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill(this.gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseSkill(gameObject);
        }
    }

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;

        LSO_PlayerMovement movement =
            player.GetComponent<LSO_PlayerMovement>();

        if (movement == null)
            return;

        Vector2 dir = movement.GetLastDir();

        if (dir == Vector2.zero)
            return;

        Vector3 spawnPos =
            player.transform.position +
            (Vector3)(dir.normalized * spawnDistance);

        GameObject poison =
            Instantiate(poisonPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = poison.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = dir.normalized * moveSpeed;

            StartCoroutine(StopPoison(rb, moveTime));
        }

        Destroy(poison, lifeTime);

        StartCoroutine(CoolTime(_coolTime));
    }

    private IEnumerator StopPoison(Rigidbody2D rb, float time)
    {
        yield return new WaitForSeconds(time);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;

        yield return new WaitForSeconds(time);

        _canUse = true;
    }
}
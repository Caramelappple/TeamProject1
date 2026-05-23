using UnityEngine;
using System.Collections;

public class FallingSpear : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float explodeDelay = 1f;

    [Header("Damage")]
    [SerializeField] private int damage = 4;
    [SerializeField] private float damageRadius = 2f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Land Effect")]
    [SerializeField] private GameObject landEffectPrefab;
    [SerializeField] private float landEffectLifeTime = 1f;
    [SerializeField] private Vector2 landEffectOffset = new Vector2(0f, -1f);

    private Vector2 targetPos;
    private bool hasTarget;
    private bool hasStopped;
    private bool hasExploded;

    [SerializeField] private float hitEffectLifeTime = 1.5f;

    public void SetTarget(Vector2 target)
    {
        targetPos = target;
        hasTarget = true;
    }

    private void Update()
    {
        if (!hasTarget || hasStopped || hasExploded) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) <= 0.05f)
        {
            transform.position = targetPos;
            hasStopped = true;

            if (landEffectPrefab != null)
            {
                Vector3 spawnPos = transform.position + (Vector3)landEffectOffset;
                GameObject landEffect = Instantiate(landEffectPrefab, spawnPos, Quaternion.identity);
                Destroy(landEffect, landEffectLifeTime);
            }

            StartCoroutine(ExplodeAfterDelay());
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explodeDelay);
        hasExploded = true;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, damageRadius, playerLayer);
        if (hit != null)
        {
            Health playerHealth = hit.GetComponent<Health>();
            if (playerHealth != null)
            {
                DamageData data = DamageData.Create(null, damage);
                playerHealth.GetDamage(data);
            }
        }

        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, hitEffectLifeTime);
        }

        Destroy(gameObject);
    }
}
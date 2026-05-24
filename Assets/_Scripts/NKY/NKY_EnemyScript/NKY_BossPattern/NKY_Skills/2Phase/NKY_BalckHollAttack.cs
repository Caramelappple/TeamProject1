using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;
using Random = UnityEngine.Random;

public class NKY_BalckHollAttack : NKY_BossSkill
{
    Rigidbody2D _targetRigid = null;
    
    [Header("설정")]
    [SerializeField] private Transform[] point;
    [SerializeField] private GameObject hole;
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private Collider2D holeCollider;
    
    [Header("스킬 설정")]
    [SerializeField] private float range;
    [SerializeField] private float pullStrength;
    [SerializeField] private float lockStrength = 1f;
    [SerializeField] private int slashCount = 7;
    
    private Vector2 _targetVelocity;
    
    private Queue<GameObject> slashQueue = new Queue<GameObject>();
    
    [field: SerializeField] public override float DamageScale { get; protected set; } = 0.4f;

    //private int _damage;

    protected override void OnAwake()
    {
        base.OnAwake();
        _damage = (int)(DamageScale * _bossBrain.Damage);
        hole.SetActive(false);
        CreateQueue();
    }

    private void CreateQueue()
    {
        for (int i = 0; i < slashCount; i++)
        {
            GameObject slash;
            slash = Instantiate(slashPrefab, transform);
            slash.SetActive(false);
            slashQueue.Enqueue(slash);
        }
    }
    
    public override IEnumerator Execute(Transform boss, Transform target)
    {
        yield return PlaySequence(
            OnHoleEffect(hole, boss.position),
            OnPull(target.gameObject),
            WaitUntilOrTime(() => false, 3f),
            Slash(point, boss, target),
            OffPull(target.gameObject),
            OffHoleEffect(hole)
            );
        _targetRigid = null;
    }

    public override void EndSkill()
    {
        _targetRigid = null;
        StartCoroutine(OffHoleEffect(hole));
        foreach (GameObject slash in slashs)
        {
            StartCoroutine(EnQueue(slash, slashQueue));
        }
    }

    private IEnumerator OnHoleEffect(GameObject effect, Vector3 position)
    {
        Animator anim = effect.GetComponent<Animator>();
        Vector2 effectScale = effect.transform.localScale;
        effect.SetActive(true);
        effect.transform.position = position;
        effect.transform.localScale = effectScale * 0.3f;
        yield return StartCoroutine(WaitUntilOrTime(() => false, 2f));
        float current = 0.3f;
        while (true)
        {
            Vector2 currentScale = new Vector2(effectScale.x * current, effectScale.y * current);
            effect.transform.localScale = currentScale;
            if (currentScale.x > effectScale.x && currentScale.y > effectScale.y)
            {
                break;
            }
            current += 3f * Time.deltaTime;
            yield return null;
        }
        
        anim.Play("BlackHole");
        yield break;
    }

    private IEnumerator OffHoleEffect(GameObject effect)
    {
        Vector2 effectScale = effect.transform.localScale;
        while (true)
        {
            effect.transform.localScale = Vector2.Lerp(effectScale, effectScale * 0.1f, -Time.deltaTime);
            yield return null;
            if(effect.transform.localScale.x <= effectScale.x && effect.transform.localScale.y <= effectScale.y)
                break;
        }
        effect.SetActive(false);
    }

    private IEnumerator OnPull(GameObject target)
    {
        _targetRigid = target.GetComponent<Rigidbody2D>();
        yield break;
    }
    
    private IEnumerator OffPull(GameObject target)
    {
        _targetRigid = null;
        yield break;
    }

    private List<GameObject> slashs = new List<GameObject>();
    private IEnumerator Slash(Transform[] point, Transform boss, Transform target)
    {
        GameObject slash;
        int pointIndex = 0;
        int oldIndex = 0;
        for (int i = 0; i < slashCount; i++)
        {
            while (true)
            {
                pointIndex = Random.Range(0, point.Length);
                if (pointIndex == oldIndex)
                {
                    continue;
                }
                break;
            }
            oldIndex = pointIndex;
            slash = slashQueue.Dequeue();
            slash.SetActive(true);
            slash.transform.position = point[pointIndex].position;
            slash.transform.rotation = Quaternion.Euler(0f, 0f, pointIndex * -90f);
            slashs.Add(slash);
            
            yield return PlaySequence(
                Attack(() => _HitBoxController.Cast(holeCollider, hole.transform.position,
                    (hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage))),
                SlashEffect(slash),
                EnQueue(slash, slashQueue));
        }
    }

    private IEnumerator EnQueue(GameObject obj, Queue<GameObject> queue)
    {
        obj.SetActive(false);
        queue.Enqueue(obj);
        yield break;
    }

    private IEnumerator SlashEffect(GameObject effect)
    {
        Animator anim;
        anim = effect.GetComponent<Animator>();
        effect.SetActive(true);
        anim = effect.GetComponent<Animator>();
        anim.Play("SlashEffect");
        yield return StartCoroutine(WaitAnim(anim, "SlashEffect", 1f));
    }
    
    void FixedUpdate()
    {
        if (_targetRigid == null) return;
        Vector2 direction = (Vector2)hole.transform.position - _targetRigid.position;
        float distance = direction.magnitude;
        if (distance <= range)
        {
            if (distance < lockStrength)
            {
                _targetRigid.transform.position = hole.transform.position;
            }
            else
            {
                Vector3 pullVelocity = direction.normalized * pullStrength;
                _targetRigid.transform.position += pullVelocity * Time.fixedDeltaTime;
            }
        }
    }
}

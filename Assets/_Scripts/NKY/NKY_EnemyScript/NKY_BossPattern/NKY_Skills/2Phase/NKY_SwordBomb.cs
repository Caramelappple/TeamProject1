using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY._EnemyScript.BossPattern;
using _Scripts.NKY.NKY_EnemyScript.NKY_Skills;
using UnityEngine;

public class NKY_SwordBomb : NKY_BossSkill
{
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject swordPrefab;
        [SerializeField] private GameObject effectPrefab;
        [SerializeField] private Collider2D bombCollider;
        private Queue<GameObject> effectQueue = new Queue<GameObject>();
        
        
        private Queue<GameObject> swordQueue = new Queue<GameObject>();
        
        [Header("보스 스킬 스텟 세팅")]
        [SerializeField] private float spawnInterval = 0.3f;
        [SerializeField] private int swordCount = 8;
        [SerializeField] private float swordMoveDuration = 0.2f;
        [field: SerializeField] public override float DamageScale { get; protected set; } = 0.7f;
        
        private int _damage;

        protected void Start()
        {
            //bombCollider = effectPrefab.GetComponent<Collider2D>();
            effectPrefab.SetActive(false);
            _damage = (int)(DamageScale * _bossBrain.Damage);
            GameObject sword;
            GameObject effect;
            for (int i = 0; i < swordCount; i++)
            {
                sword = Instantiate(swordPrefab, transform);
                sword.SetActive(false);
                swordQueue.Enqueue(sword);

                effect = Instantiate(effectPrefab, transform);
                effect.SetActive(false);
                effectQueue.Enqueue(effect);
            }
        }

        private List<GameObject> swords = new List<GameObject>();
        private List<GameObject> effects =  new List<GameObject>();
        
        public override IEnumerator Execute(Transform boss, Transform target)
        {
            GameObject sword = null;
            GameObject effect = null;
            Vector3 moveDir;
            int spawnRange;
            for (int i = 0; i < swordCount; i++)
            {
                Vector3 pos = target.position;
                spawnRange = Random.Range(0, spawnPoints.Length);
                sword = swordQueue.Dequeue();
                sword.GetComponent<NKY_CrashDamage>().damage = _damage;
                sword.transform.parent = spawnPoints[spawnRange];
                sword.transform.position = spawnPoints[spawnRange].position;
                moveDir = (target.transform.position - sword.transform.position).normalized;
                sword.transform.up = moveDir;
                swords.Add(sword);
                sword.SetActive(true);

                effect = effectQueue.Dequeue();
                effects.Add(effect);
                
                StartCoroutine(PlaySequence(
                    ShowWarn(0, new Vector2(0.2f , (pos - sword.transform.position).magnitude),
                        0.8f, () => Vector3.Lerp(sword.transform.position, pos, 0.5f), sword.transform.localEulerAngles.z),
                    WaitUntilOrTime(() => false, 0.6f),
                    MoveTo(sword.transform, pos, swordMoveDuration),
                    WaitUntilOrTime(() => false, swordMoveDuration),
                    ShowWarn(bombCollider, 0.5f, () => pos),
                    WaitUntilOrTime(() => false, 0.5f),
                    EnQueues(sword, swordQueue),
                    PlayBombEffect(effect, pos),
                    Attack(() =>
                    {
                        _HitBoxController.Cast(bombCollider, pos,
                            (hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage));
                    }),
                    EnQueues(effect, effectQueue)
                ));

                yield return new WaitForSeconds(spawnInterval);
            }
            yield break;
        }

        public override void EndSkill()
        {
            foreach (GameObject sword in swords)
            {
                StartCoroutine(EnQueues(sword, swordQueue));
            }
            foreach (GameObject effect in effects)
            {
                StartCoroutine(EnQueues(effect, effectQueue));
            }
        }

        private IEnumerator EnQueues(GameObject obj, Queue<GameObject> queue)
        {
            obj.SetActive(false);
            queue.Enqueue(obj);
            yield break;
        }
        
        private IEnumerator PlayBombEffect(GameObject effect, Vector2 pos)
        {
            Animator effectAnim =  effect.GetComponent<Animator>();
            effect.transform.position = pos;
            effect.SetActive(true);
            effectAnim.Play("BombEffectAnim");
            yield return WaitAnim(effectAnim, "BombEffectAnim", 0.9f);
        }
}

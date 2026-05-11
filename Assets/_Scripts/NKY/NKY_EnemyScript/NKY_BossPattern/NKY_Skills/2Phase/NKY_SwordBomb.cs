using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY._EnemyScript;
using _Scripts.NKY._EnemyScript.BossPattern;
using _Scripts.NKY.NKY_EnemyScript.NKY_Skills;
using UnityEngine;

public class NKY_SwordBomb : NKY_BossSkill
{
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject swordPrefab;
        [SerializeField] private Collider2D bombCollider;
        private Queue<GameObject> swordQueue = new Queue<GameObject>();
        
        [Header("보스 스킬 스텟 세팅")]
        [SerializeField] private float spawnInterval = 0.3f;
        [SerializeField] private int swordCount = 8;
        [SerializeField] private float swordDuration = 0.5f;
        [field: SerializeField] public override float damageScale { get; protected set; } = 0.7f;
        
        private int _damage;

        protected void Start()
        {
            _damage = (int)damageScale * _bossBrain.GetComponent<NKY_Enemy>().damage;
            GameObject sword;
            for (int i = 0; i < swordCount; i++)
            {
                sword = Instantiate(swordPrefab, transform);
                sword.SetActive(false);
                swordQueue.Enqueue(sword);
            }
        }

        public override IEnumerator Execute(Transform boss, Transform target)
        {
            GameObject sword = null;
            Vector3 moveDir;
            int spawnRange;
                
            for (int i = 1; i < swordCount; i++)
            {
                spawnRange = Random.Range(0, spawnPoints.Length);
                sword = swordQueue.Dequeue();
                sword.GetComponent<NKY_CrashDamage>().damage = _damage;
                sword.transform.parent = spawnPoints[spawnRange];
                sword.transform.position = spawnPoints[spawnRange].position;
                moveDir = (target.transform.position - sword.transform.position).normalized;
                sword.transform.up = moveDir;
                sword.SetActive(true);


                StartCoroutine(PlaySequence(
                    ShowWarn(0, new Vector2(0.2f, (target.position - sword.transform.position).magnitude),
                        0.8f, () => Vector3.Lerp(sword.transform.position, target.position, 0.5f), sword.transform.localEulerAngles.z),
                    WaitUntilOrTime(() => false, 1f),
                    MoveTo(sword.transform, target.transform.position, swordDuration),
                    ShowWarn(bombCollider, 0.5f, () => sword.transform.position),
                    WaitUntilOrTime(() => false, 0.5f),
                    EnQueues(sword),
                    Attack(() => _HitBoxController.Cast(bombCollider, (hitTarget) => HitToDamage(hitTarget, _damage)))
                ));

                yield return new WaitForSeconds(spawnInterval);
            }
            yield break;
        }


        private IEnumerator EnQueues(GameObject obj)
        {
            obj.SetActive(false);
            swordQueue.Enqueue(obj);
            yield break;
        }
}

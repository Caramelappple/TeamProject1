using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

namespace _Scripts.NKY.NKY_EnemyScript.NKY_Skills
{
    public class NKY_SwordStorm : NKY_BossSkill
    {
        [SerializeField] private Transform[] spawnPoints;
        public GameObject swordPrefab;
        private Queue<GameObject> swordQueue = new Queue<GameObject>();
        
        [SerializeField] private float spawnInterval = 0.3f;
        [SerializeField] private int swordCount = 8;
        [SerializeField] private float swordDistance = 35;
        [SerializeField] private float swordDuration = 0.5f;

        protected void Start()
        {
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
                sword.transform.parent = spawnPoints[spawnRange];
                sword.transform.position = spawnPoints[spawnRange].position;
                moveDir = (target.transform.position - sword.transform.position).normalized;
                sword.transform.up = moveDir;
                sword.SetActive(true);

                BoxCollider2D col = sword.GetComponentInChildren<BoxCollider2D>();
                Vector2 offset = col.size * 0.5f;
                //yield return StartCoroutine(ShowWarn(col, 0.6f, () => sword.transform.position + (moveDir * offset.y)));
                StartCoroutine(PlaySequence(
                    ShowWarn(col, 0.3f, () => sword.transform.position + (moveDir * offset.y)),
                    WaitUntilOrTime(() => false, 0.6f),
                    Move(sword.transform, sword.transform.up, swordDistance, swordDuration),
                    WaitUntilOrTime(() => false, 1),
                    EnQueues(sword)
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
}

using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _Scripts.NKY._EnemyScript.Skills
{
    public class SwordStorm : BossSkill
    {
        [SerializeField] private Transform[] spawnPoints;
        public GameObject swordPrefab;
        private Queue<GameObject> swordQueue = new Queue<GameObject>();
        
        [SerializeField] private float spawnInterval = 0.3f;
        [SerializeField] private int swordCount = 8;
        [SerializeField] private float swordDistance = 35;
        [SerializeField] private float swordDuration = 0.5f;

        protected override void OnAwake()
        {
            GameObject sword;
            for (int i = 0; i < swordCount; i++)
            {
                sword = Instantiate(swordPrefab);
                sword.SetActive(false);
                swordQueue.Enqueue(sword);
            }
        }

        public override IEnumerator Execute(Transform boss, Transform target)
        {
            GameObject sword = null;
            Vector3 moveDir;
            
            for (int i = 0; i < swordCount - 1; i++)
            {
                sword = swordQueue.Dequeue();
                sword.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                moveDir = (target.transform.position - sword.transform.position).normalized;
                sword.transform.up = moveDir;
                sword.SetActive(true);

                BoxCollider2D col = sword.GetComponentInChildren<BoxCollider2D>();
                Vector2 offset = col.size * 0.5f;
                //yield return StartCoroutine(ShowWarn(col, 0.6f, () => sword.transform.position + (moveDir * offset.y)));
                PlaySequence(
                    ShowWarn(col, 0.6f, () => sword.transform.position + (moveDir * offset.y)),
                    WaitUntilOrTime(() => false, 0.6f),
                    Move(sword.transform, sword.transform.up, swordDistance, swordDuration),
                    WaitUntilOrTime(() => false, 1),
                    EnQueues(sword)
                );

                // 다음 칼을 생성하기 전까지 일정 시간(spawnInterval) 대기합니다.
                yield return new WaitForSeconds(spawnInterval);
            }
        }


        private IEnumerator EnQueues(GameObject obj)
        {
            obj.SetActive(false);
            swordQueue.Enqueue(obj);
            yield break;
        }
    }
}

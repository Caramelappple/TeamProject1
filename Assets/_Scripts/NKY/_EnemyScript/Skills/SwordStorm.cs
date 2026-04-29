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
            GameObject sword;
            Vector3 moveDir;
            for (int i = 0; i < swordCount; i++)
            {
                sword = swordQueue.Dequeue();
                sword.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                moveDir = (target.transform.position - sword.transform.position).normalized;
                sword.transform.up = moveDir;
                sword.SetActive(true);


                StartCoroutine(SwordActionRoutine(sword, moveDir));

                // 다음 칼을 생성하기 전까지 일정 시간(spawnInterval) 대기합니다.
                yield return new WaitForSeconds(spawnInterval);
            }

            yield break;
        }
        private IEnumerator SwordActionRoutine(GameObject sword, Vector3 direction)
        {
            BoxCollider2D col = sword.GetComponentInChildren<BoxCollider2D>();
            Vector2 offset = col.size * 0.5f;
            // PlaySequence가 끝날 때까지 이 코루틴 안에서만 대기합니다.
            // 메인 Execute 코루틴의 for문에는 영향을 주지 않습니다.
            yield return PlaySequence(
                ShowWarn(col, 0.6f, () => sword.transform.position + (direction * offset.y)),
                WaitUntilOrTime(() => false, 0.6f),
                Move(sword.transform, sword.transform.up, 35, 0.5f),
                EnQueues(sword)
            );
        }

        private IEnumerator EnQueues(GameObject obj)
        {
            obj.SetActive(false);
            swordQueue.Enqueue(obj);
            yield break;
        }
    }
}

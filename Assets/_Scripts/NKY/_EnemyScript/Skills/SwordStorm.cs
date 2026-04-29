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

        protected override void OnAwake()
        {
            GameObject sword;
            for (int i = 0; i < 8; i++)
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
            for (int i = 0; i < 8; i++)
            {
                sword = swordQueue.Dequeue();
                sword.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                moveDir = (target.transform.position - sword.transform.position).normalized;
                sword.transform.up = moveDir;
                sword.SetActive(true);


                StartCoroutine(SwordActionRoutine(sword));

                // 다음 칼을 생성하기 전까지 일정 시간(spawnInterval) 대기합니다.
                yield return new WaitForSeconds(spawnInterval);
            }

            yield break;
        }
        private IEnumerator SwordActionRoutine(GameObject sword)
        {
            // PlaySequence가 끝날 때까지 이 코루틴 안에서만 대기합니다.
            // 메인 Execute 코루틴의 for문에는 영향을 주지 않습니다.
            yield return PlaySequence(
                ShowWarn(sword.GetComponent<Collider2D>(), 0.6f, () => sword.transform.position),
                WaitUntilOrTime(() => false, 0.6f),
                Move(sword.transform, sword.transform.up, 40, 1),
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

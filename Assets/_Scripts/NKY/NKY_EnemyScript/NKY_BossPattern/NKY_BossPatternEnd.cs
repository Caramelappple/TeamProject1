using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.NKY._EnemyScript.BossPattern
{
    public class NKY_BossPatternEnd : MonoBehaviour
    {
        private WaitForSeconds wait;
        private void Start()
        {
            wait = new WaitForSeconds(2f);
        }

        public IEnumerator BossPatternEnd(Transform boss)
        {
            Vector3 endPos = new Vector3(Random.Range(-7f, 17f), Random.Range(-14f, 5f), 0);
            boss.DOMove(endPos, 2f);
            yield break;
        }
    }
}
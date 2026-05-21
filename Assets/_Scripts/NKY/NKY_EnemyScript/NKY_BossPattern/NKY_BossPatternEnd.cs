using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.NKY._EnemyScript.BossPattern
{
    public class NKY_BossPatternEnd : MonoBehaviour
    {
        public IEnumerator BossPatternEnd(Transform boss)
        {
            //Vector3 endPos = new Vector3(Random.Range());
            //boss.position = new Vector3(boss.position.x, boss.position.y, boss.position.z);
            yield break;
        }
    }
}
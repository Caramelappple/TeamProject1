using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY._EnemyScript;
using _Scripts.NKY._EnemyScript.BossPattern;
using _Scripts.NKY.NKY_EnemyScript.NKY_Skills;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NKY_ReturnSwordAttack : NKY_BossSkill
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject swordPrefab;
        
    [Header("보스 스킬 스텟 세팅")]
    [SerializeField] private float spawnInterval = 0.3f;
    [SerializeField] private int swordCount = 6;
    [SerializeField] private float swordDuration = 0.2f;
    
    private Queue<GameObject> swordQueue = new Queue<GameObject>();
    [field: SerializeField] public override float DamageScale { get; protected set; } = 1f;
    private int _damage;
    protected void Start()
    {
        _damage = (int)(DamageScale * _bossBrain.damage);
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
        GameObject[] swords = new GameObject[swordCount];
        for (int i = 0; i < swordCount; i++)
        {
            swords[i] = swordQueue.Dequeue();
            swords[i].GetComponent<NKY_CrashDamage>().damage = _damage;
        }
        Vector3 moveDir;
        foreach (GameObject sword in swords)
        {
            Vector3 pos = target.position;
            
            sword.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            moveDir = (pos - sword.transform.position).normalized;
            sword.transform.up = moveDir;
            sword.SetActive(true);

            yield return PlaySequence(
                ShowWarn(0, new Vector2(0.4f , (pos - sword.transform.position).magnitude),
                    0.8f, () => Vector3.Lerp(sword.transform.position, pos, 0.5f), sword.transform.localEulerAngles.z),
                WaitUntilOrTime(() => false, 0.2f),
                MoveTo(sword.transform, target.position, swordDuration)
            );
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(0.5f);
        foreach (GameObject sword in swords)
        {
            StartCoroutine(ReturnSword(sword, boss));
        }
        yield return new WaitForSeconds(1f);
        foreach (GameObject sword in swords)
        {
            StartCoroutine(ShowWarn(0, new Vector2(0.4f, (boss.transform.position - sword.transform.position).magnitude),
                0.8f, () => Vector3.Lerp(sword.transform.position, boss.transform.position, 0.5f),
                sword.transform.localEulerAngles.z));
        }
        yield return new WaitForSeconds(0.9f);
        foreach (GameObject sword in swords)
        {
            StartCoroutine(MoveTo(sword.transform, boss.position, swordDuration));
        }
        yield return new WaitForSeconds(3f);
        EnQueues(swords, swordQueue);
    }

    private IEnumerator ReturnSword(GameObject sword, Transform target)
    {
        Vector3 dir = target.position - sword.transform.position;
        
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        angle -= 90f; 

        Quaternion rot = Quaternion.Euler(0, 0, angle);
        while (true)
        {
            if (Quaternion.Angle(sword.transform.rotation, rot) < 0.1f)
            {
                sword.transform.rotation = rot;
                break;
            }
            sword.transform.rotation = Quaternion.Slerp(
                sword.transform.rotation, 
                rot, 
                Time.deltaTime * 8f
            );
    
            yield return null;
        }
    }
    
    private void EnQueues(GameObject[] objs, Queue<GameObject> queue)
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY._EnemyScript.BossPattern;
using _Scripts.NKY.NKY_EnemyScript.NKY_Skills;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class NKY_ReturnSwordAttack : NKY_BossSkill
{
    [Header("?????")] 
    [SerializeField] private NKY_SoundData throwSword;
    
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Transform returnTarget;
    [SerializeField] private string playAnimName = "";
    [SerializeField] private bool bul;
        
    [Header("???? ??? ???? ????")]
    [SerializeField] private float spawnInterval = 0.3f;
    [SerializeField] private int swordCount = 6;
    [SerializeField] private float swordDuration = 0.2f;
    
    private Queue<GameObject> swordQueue = new Queue<GameObject>();
    [field: SerializeField] public override float DamageScale { get; protected set; } = 1f;
    private List<GameObject> swords =  new List<GameObject>();
    //private int _damage;
    protected void Start()
    {
        returnTarget = NKY_GameManager.instance.player.transform;

        _damage = (int)(DamageScale * _bossBrain.Damage);
        for (int i = 0; i < swordCount; i++)
        {
            GameObject sword;
            sword = Instantiate(swordPrefab, transform);
            sword.SetActive(false);
            swordQueue.Enqueue(sword);
        }
    }
    public override IEnumerator Execute(Transform boss, Transform target)
    {
        Vector3 pos = returnTarget.position; 
        Vector3 moveDir;
        float distance;
        float fireAngle = 0;
        swords.Clear();
        GameObject[] _swords = new GameObject[swordCount];
        for (int i = 0; i < swordCount; i++)
        {
            _swords[i] = swordQueue.Dequeue();
            _swords[i].GetComponent<NKY_CrashDamage>().damage = _damage;
            swords.Add(_swords[i]);
        }

        if (playAnimName != null)
        {
            if ((target.position - boss.position).x < 0)
            {
                boss.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                boss.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        foreach (GameObject sword in _swords)
        {
            Vector3 to;
            float currentAngle;
            moveDir = (target.position - sword.transform.position).normalized;
            sword.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            if (bul)
            {
                Anim.Play(playAnimName);
                distance = Mathf.Abs(Vector3.Distance(sword.transform.position, pos)) * 2;
                fireAngle += 360 / (float)swordCount;
                sword.transform.rotation = Quaternion.Euler(0, 0, fireAngle);
                to = sword.transform.position + (sword.transform.up * distance);
                currentAngle = fireAngle;
            }
            else
            {
                sword.transform.up = moveDir;
                to = target.position;
                currentAngle = sword.transform.localEulerAngles.z;
                distance = Mathf.Abs(Vector3.Distance(sword.transform.position, target.position));
            }
            sword.SetActive(true);
            
            NKY_SoundManager.Instance.PlaySFX(throwSword.soundName);//?????????? ????? ????
            
            yield return PlaySequence(
                ShowWarn(0, new Vector2(0.4f , distance),
                    0.8f, () => Vector3.Lerp(sword.transform.position, to, 0.5f), currentAngle),
                WaitUntilOrTime(() => false, 0.2f),
                Move(sword.transform, sword.transform.up, distance, swordDuration)
            );
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(0.5f);
        foreach (GameObject sword in _swords)
        {
            StartCoroutine(ReturnSword(sword, returnTarget));
        }
        yield return new WaitForSeconds(1f);
        Vector3 curPos = returnTarget.position;
        
        foreach (GameObject sword in _swords)
        {
            distance = Vector3.Distance(sword.transform.position, curPos);
            StartCoroutine(ShowWarn(0, new Vector2(0.4f, distance),
                0.8f, () => Vector3.Lerp(sword.transform.position, curPos, 0.5f),
                sword.transform.localEulerAngles.z));
        }
        yield return new WaitForSeconds(0.9f);
        foreach (GameObject sword in _swords)
        {
            StartCoroutine(MoveTo(sword.transform, curPos, swordDuration * 0.5f,Ease.InBack));
            
            NKY_SoundManager.Instance.PlaySFX(throwSword.soundName);//??????????? ????? ????
        }
        yield return new WaitForSeconds(swordDuration);
        EnQueues(_swords, swordQueue);
    }

    public override void EndSkill()
    {
        foreach (GameObject obj in swords)
        {
            obj.SetActive(false);
            swordQueue.Enqueue(obj);
        }
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
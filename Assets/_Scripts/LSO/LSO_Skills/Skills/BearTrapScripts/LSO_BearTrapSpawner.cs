using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSO_BearTrapSpawner : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private AudioClip clip;
    
    public static LSO_BearTrapSpawner instance;
    [SerializeField]private float coolTime = 1f;
    private bool _canSpawn = true;
    [SerializeField] private int _trapCount = 5;
    public readonly Stack<GameObject> trapPool = new Stack<GameObject>();
    public LinkedList<GameObject> activeTraps = new LinkedList<GameObject>();
    [SerializeField] private GameObject trapPrefab;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateTrap();
    }
    
    private void OnDestroy()
    {
        trapPool.Clear();
        activeTraps.Clear();
    }
    public void CreateTrap()
    {
        for (int i = 0; i < _trapCount; i++)
        {
            GameObject trap = Instantiate(trapPrefab,transform);
            trap.SetActive(false);  
            trapPool.Push(trap);
        }
    }

    private void SpawnTrap(GameObject player)
    {
        if (!_canSpawn) return;
        
        LSO_SoundManager.Instance.SfxPlay(clip);
        GameObject trap;

        if (trapPool.Count > 0)
        {
            trap = trapPool.Pop();
            if (trap == null) return;

            trap.SetActive(true);
            // 노드를 저장해서 트랩 컴포넌트에 전달
            var node = activeTraps.AddLast(trap);
            trap.GetComponent<LSO_BearTrap>().node = node;
        }
        else
        {
            if (activeTraps.Count == 0) return;

            var firstNode = activeTraps.First;
            trap = firstNode.Value;

            if (trap == null)
            {
                activeTraps.RemoveFirst();
                return;
            }

            activeTraps.Remove(firstNode);
            var node = activeTraps.AddLast(trap);
            trap.GetComponent<LSO_BearTrap>().node = node;
        }

        trap.transform.position = player.transform.position;
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(coolTime));
    }
    public void UseSkill(GameObject player)
    {
        SpawnTrap(player);
    }
    
    public IEnumerator CoolTime(float time)
    {
        _canSpawn = false;
        yield return new WaitForSeconds(coolTime);
        _canSpawn = true;
    }
}

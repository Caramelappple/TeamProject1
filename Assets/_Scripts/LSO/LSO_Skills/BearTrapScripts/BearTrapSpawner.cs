using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BearTrapSpawner : MonoBehaviour,ISkill
{
    public static BearTrapSpawner instance;
    private float coolTime = 1f;
    private bool canSpawn = true;
    private int trapCount = 5;
    public Stack<GameObject> trapPool = new Stack<GameObject>();
    public LinkedList<GameObject> activeTraps = new LinkedList<GameObject>();
    [SerializeField] private GameObject trapPrefab;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        CreateTrap(trapCount);
    }

    public void UseSkill()
    {
        SpawnTrap();
    }
    
    private void CreateTrap(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject trap = Instantiate(trapPrefab);
            trap.SetActive(false);  
            trapPool.Push(trap);
        }
    }

    private void SpawnTrap()
    {
        GameObject trap;
        if (trapPool.Count > 0)
        {
            trap = trapPool.Pop();
            trap.SetActive(true);
            activeTraps.AddLast(trap);
        }
        else//트랩이 모두 사용 중일 때, 가장 오래된 트랩을 재배치
        {
            trap = activeTraps.First.Value;
            activeTraps.RemoveFirst();//마지막으로 설치한 트랩 제거
            activeTraps.AddLast(trap);
        }

        trap.transform.position = transform.position;

        StartCoroutine(ReloadCoroutine());
    }
    private IEnumerator ReloadCoroutine()
    {
        canSpawn = false;
        yield return new WaitForSeconds(coolTime);
        canSpawn = true;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class JHY_MobSummoner : MonoBehaviour
{
    [SerializeField] private GameObject[] mobPrefabs;
    [SerializeField] private Transform[] summonPoints;

    private readonly List<GameObject> summonedMobs = new List<GameObject>();

    public void SummonMobs()
    {
        if (mobPrefabs.Length == 0 || summonPoints.Length == 0) return;

        summonedMobs.Clear();

        foreach (Transform point in summonPoints)
        {
            int index = Random.Range(0, mobPrefabs.Length);
            GameObject mob = Instantiate(mobPrefabs[index], point.position, Quaternion.identity);
            summonedMobs.Add(mob);
        }
    }

    public bool HasAliveSummons()
    {
        summonedMobs.RemoveAll(mob => mob == null);
        return summonedMobs.Count > 0;
    }
}

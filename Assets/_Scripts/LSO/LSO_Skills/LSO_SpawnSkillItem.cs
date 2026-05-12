using UnityEngine;
using Random = UnityEngine.Random;

public class LSO_SpawnSkillItem : MonoBehaviour
{
   [SerializeField] private GameObject[] skillPrefab;
   [SerializeField] private Transform[] spawnPoints;

   private int _point;
   private int _skillID;
   
   public static LSO_SpawnSkillItem instance;

   private void Awake()
   {
      instance = this;
   }

   private void Start()
   {
      for (int i = 0; i < spawnPoints.Length; i++)
      {
         Spawn(i);
      }
   }
   
   public void Spawn(int point)
   {
         _point = point;
         _skillID = Random.Range(0, skillPrefab.Length);

         GameObject skillItem = Instantiate(skillPrefab[_skillID], spawnPoints[_point].position, Quaternion.identity);
         skillItem.transform.parent = spawnPoints[_point].transform;
         skillItem.transform.position =  spawnPoints[_point].position;
   }
}

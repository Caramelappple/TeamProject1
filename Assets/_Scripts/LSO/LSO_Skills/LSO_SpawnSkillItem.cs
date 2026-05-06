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
      Spawn(1);
   }
   
   public void Spawn(int point)
   {
         _point = point;
         _skillID = Random.Range(0, skillPrefab.Length);

         GameObject skillItem = Instantiate(skillPrefab[_skillID], spawnPoints[_point].position, Quaternion.identity);
         skillItem.transform.parent = spawnPoints[_point].transform;
         skillItem.transform.position = new Vector3(spawnPoints[_point].position.x + 2,spawnPoints[_point].position.y,spawnPoints[_point].position.z);
         
         _skillID = Random.Range(0, skillPrefab.Length);
         
         GameObject skillItem2 = Instantiate(skillPrefab[_skillID], spawnPoints[_point].position, Quaternion.identity);
         skillItem2.transform.parent = spawnPoints[_point].transform;
         skillItem2.transform.position = new Vector3(spawnPoints[_point].position.x -2,spawnPoints[_point].position.y,spawnPoints[_point].position.z);
         
         GameObject skillGroup = new GameObject("SkillGroup");
         skillGroup.transform.position = spawnPoints[_point].position;

         skillItem.transform.parent = skillGroup.transform;
         skillItem2.transform.parent = skillGroup.transform;
   }
}

using LSO.SkillSystem;
using UnityEngine;

public class LSO_SkillItemSpawner : MonoBehaviour
{
   //포인트 세릴라이즈 필드로 가져오기
   //포인트에 스폰할 스킬 아이템 목록 만들기(SO 형식으로)
   //포인트에 스폰하기
   //같은 부모인 스폰포인트들의 수를 구하기
   //먹으면 포인트에 할당된 아이템 지우기
   
   
   /*[SerializeField]private GameObject skillPrefab;
   private Transform[] spawnPoints;
   [SerializeField]private LSO_SkillResourceSO[] skillItems;
   
   private int _pointIndex;
   private int _skillIndex;

   private void Start()
   {
      spawnPoints = gameObject.GetComponentsInChildren<Transform>();
      _pointIndex = spawnPoints.Length;
   }

   private void SpawnSkillItems()
   {
      for (int i = 0; i < spawnPoints.Length; i++)
      {
         _skillIndex = Random.Range(0, skillItems.Length);

         GameObject skillItem = Instantiate(skillPrefab);
         skillItem.transform.position = spawnPoints[i].position;

         LSO_SkillItem skillItemScript = skillItem.GetComponent<LSO_SkillItem>();
         skillItemScript.skillData = skillItems[_skillIndex];
      }
   }*/
}

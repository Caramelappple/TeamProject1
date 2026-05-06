using KHG.Player;
using UnityEngine;


namespace KHG.Enemy
{
    public class KHG_Enemy : MonoBehaviour
    {
        private int _damage = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            KHG_Health health;
            if (collision.gameObject.TryGetComponent<KHG_Health>(out health))
            {

                
                
               
                
            }
        }
        public void Test1(KHG_DamageData args)
        {
            

            
            
           
        }
        public void HitHandler(int damage, GameObject giver)
        {
            Debug.Log("나 피가 달았어요!");
        }
    }
}

    
   

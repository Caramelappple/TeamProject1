using System;
using KDH.ItemSystem;
using UnityEngine;

namespace KHG.Player
{
    public class KHG_Player : MonoBehaviour
    {
        public KHG_Health Health { get; private set; }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            
            //아이템에 닿았을 때 아이템을 먹는 동작
            if (other.gameObject.TryGetComponent(out KHG_ICollectable iCollectable))
            {
                iCollectable.Collect(this);
            }
            
        }
    }   
}
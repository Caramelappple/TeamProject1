using System;
using KDH.ItemSystem;
using KHG.ItemSystem;
using UnityEngine;

namespace KHG.Player
{
    public class KHG_Player : MonoBehaviour
    {
        public Health Health { get; private set; }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out KHG_ICollectable iCollectable))
            {
                iCollectable.Collect(this);
            }
            
        }
    }   
}
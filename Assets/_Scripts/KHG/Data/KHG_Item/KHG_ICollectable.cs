using System;
using KHG.Player;
using UnityEngine;

namespace KHG.ItemSystem
{
    public interface KHG_ICollectable 
    {
        public event Action<KHG_ICollectable> OnCollected;
        public void Collect(KHG_Player Collector);
    }
}
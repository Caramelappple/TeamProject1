using UnityEngine;

namespace KHG.HealthSystme
{
    public readonly struct KHG_RecoverData
    {
        public readonly KHG_Entity giver;
        public readonly int recoverValue;
        
        public KHG_RecoverData(KHG_Entity giver, int resourceValue)
        {
            this.giver = giver;
            this.recoverValue = resourceValue;
        }

        public static KHG_RecoverData Create(KHG_Entity giver, int resourceValue)
        {
            KHG_RecoverData result = new KHG_RecoverData(giver, resourceValue);
            return result;
        }
    }

    public readonly struct KHG_RecoverResultData
    {
        public readonly KHG_Entity giver;
        public readonly int recoverValue;
        public readonly int resourceValue;

        public KHG_RecoverResultData(KHG_Entity giver, int recoverValue, int resourceData)
        {
            this.giver = giver;
            this.recoverValue = recoverValue;
            this.resourceValue = resourceData;
        }

        public static KHG_RecoverResultData Create(KHG_Entity giver, int resourceValue, int resourceData)
        {
            KHG_RecoverResultData result = new KHG_RecoverResultData(giver, resourceValue, resourceData);
            return result;
        }
    }
}




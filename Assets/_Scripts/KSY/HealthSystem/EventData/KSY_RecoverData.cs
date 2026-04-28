using Assets._Scripts.KSY;
using UnityEngine;

namespace KSY.HealthSystem
{
    //회복시 넘겨주는 값
    public readonly struct KSY_RecoverData
    {
        public readonly Assets._Scripts.KSY.KSY_Entity giver;
        public readonly int recoverValue;

        public KSY_RecoverData(Assets._Scripts.KSY.KSY_Entity giver, int recoverValue)
        {
            this.giver = giver;
            this.recoverValue = recoverValue;
        }
        public static KSY_RecoverData Create(Assets._Scripts.KSY.KSY_Entity giver, int recoverValue)
        {
            KSY_RecoverData result = new KSY_RecoverData(giver, recoverValue);
            return result;
        }
    }
    //회복 했을 때 이벤트로 넘겨주는 값
    public readonly struct KSY_RecoverResultData
    {
        public readonly Assets._Scripts.KSY.KSY_Entity giver;
        public readonly int recoverValue;
        public readonly int resourceData;

        public KSY_RecoverResultData(Assets._Scripts.KSY.KSY_Entity giver, int recoverValue, int resourceData)
        {
            this.giver = giver;
            this.recoverValue = recoverValue;
            this.resourceData = resourceData;
        }
        public static KSY_RecoverResultData Create(Assets._Scripts.KSY.KSY_Entity giver, int recoverValue, int resourceData)
        {
            KSY_RecoverResultData result = new KSY_RecoverResultData(giver, recoverValue, resourceData);
            return result;
        }
    }
}


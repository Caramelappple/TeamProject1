using UnityEngine;

namespace KDH_HealthSystem
{
    public readonly struct KDH_RecoverData
    {
        //"EventArgs"는 EventArguments의 약자, 참조 형식은 힙
        //회복시 넘겨주는 값
        public readonly Entity giver;
        public readonly int recoverValue;


        public KDH_RecoverData(Entity giver, int recoverValue)
        {
            this.giver = giver;
            this.recoverValue = recoverValue;
        }
        public static KDH_RecoverData Create(Entity giver, int recoverValue)
        {
            KDH_RecoverData result = new KDH_RecoverData(giver, recoverValue);
            return result;
        }

        //회복 했을 때 이벤트로 넘겨주는 값
        public readonly struct KDH_RecoverResultData
        {
            public readonly Entity giver;
            public readonly int recoverValue;
            public readonly int resourceData;


            public KDH_RecoverResultData(Entity giver, int recoverValue, int resourceData)
            {
                this.giver = giver;
                this.recoverValue = recoverValue;
                this.resourceData = resourceData;
            }
            public static KDH_RecoverResultData Create(Entity giver, int recoverValue, int resourceData)
            {
                KDH_RecoverResultData result = new KDH_RecoverResultData(giver, recoverValue, resourceData);
                return result;
            }
        }
    }
}
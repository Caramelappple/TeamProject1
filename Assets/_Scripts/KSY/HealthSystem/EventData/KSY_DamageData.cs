using Assets._Scripts.KSY;

namespace KSY.HealthSystem
{
    //이벤트 발생 시 전달되는 매개변수(데이터)를 담는 객체
    public readonly struct KSY_DamageData
    {
        public readonly KDH_Entity giver;
        public readonly int damage;

        public KSY_DamageData(KDH_Entity giver, int damage)
        {
            this.giver = giver;
            this.damage = damage;
        }
        public static KSY_DamageData Create(KDH_Entity giver, int damage)
        {
            KSY_DamageData result = new KSY_DamageData(giver, damage);
            return result;
        }
    }

    public readonly struct KSY_DamageResultData
    {
        public readonly KDH_Entity giver;
        public readonly int damage;
        public readonly int resourceValue;

        public KSY_DamageResultData(KDH_Entity giver, int damage, int resourceValue)
        {
            this.giver = giver;
            this.damage = damage;
            this.resourceValue = resourceValue;
        }
        public static KSY_DamageResultData Create(KDH_Entity giver, int damage, int resourceValue)
        {
            KSY_DamageResultData result = new KSY_DamageResultData(giver, damage, resourceValue);
            return result;
        }
    }
}

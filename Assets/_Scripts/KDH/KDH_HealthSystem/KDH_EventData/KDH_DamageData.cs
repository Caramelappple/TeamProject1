using KSY_HealthSystem;
using UnityEngine;

//이벤트 발생 시 전달되는 매개변수(데이터)를 담는 객체
//구조체는 모든 매개변수를 초기화를 해줘야 함.
public readonly struct KDH_DamageData
{
    public readonly KDH_Entity giver;
    public readonly int damage;

    public KDH_DamageData(KDH_Entity giver, int damage)
    {
        this.giver = giver;
        this.damage = damage;
    }
    public static KDH_DamageData Create(KDH_Entity giver, int damage)
    {
        KDH_DamageData result = new KDH_DamageData(giver, damage);
        return result;
    }
    public void Create1()
    {
        KDH_DamageData asd = new KDH_DamageData();
        asd.Create1();

        KDH_DamageData.Create(null, 1);
    }
}

public readonly struct KDH_DamageResultData
{
    public readonly KDH_Entity giver;
    public readonly int damage;
    public readonly int resourceValue;

    public KDH_DamageResultData(KDH_Entity giver, int damage, int resourceValue)
    {
        this.giver = giver;
        this.damage = damage;
        this.resourceValue = resourceValue;
    }
    public static KDH_DamageResultData Create(KDH_Entity giver, int damage, int resourceValue)
    {
        KDH_DamageResultData result = new KDH_DamageResultData(giver, damage, resourceValue);
        return result;
    }
}
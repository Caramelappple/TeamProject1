using KHG.HealthSystme;
using System;
using System.Collections;
using UnityEngine;

public struct KHG_DamageData
{
    public KHG_Entity giver;
    public int damage;

    //접근제한자 생성하려는자료이름()
    public KHG_DamageData(KHG_Entity giver, int damage)
    {
        this.giver = giver;
        this.damage = damage;
    }
    public static KHG_DamageData Create(KHG_Entity giver, int damage)
    {
        KHG_DamageData result = new KHG_DamageData(giver, damage);
        return result;
    }

    internal static KHG_DamageData Create(object giver, object damageValue)
    {
        throw new NotImplementedException();
    }
}
public struct KHG_DamageResultData
{
    public readonly KHG_Entity giver;
    public readonly int damage;
 
    public KHG_DamageResultData(KHG_Entity giver, int damage)
    {
        this.giver = giver;
        this.damage = damage;
    }

    public static KHG_DamageResultData Create(KHG_Entity giver, int damage)
    {
        KHG_DamageResultData result = new KHG_DamageResultData(giver, damage);
        return result;
    }
    
}
public readonly struct KHG_RecoverResultData
{
    public readonly KHG_Entity giver;
    public readonly int damamge;
    public readonly int resourceValue;

    public KHG_RecoverResultData(KHG_Entity giver, int damamge, int resourceData)
    {
        this.giver = giver;
        this.damamge = damamge;
        this.resourceValue = resourceData;
    }

    public static KHG_RecoverResultData Create(KHG_Entity giver, int damage,int resourceValue)
    {
        KHG_RecoverResultData result = new KHG_RecoverResultData(giver, damage, resourceValue);
        return result;
    }
}

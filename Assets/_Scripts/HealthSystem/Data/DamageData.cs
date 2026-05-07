//이거 사용
using UnityEngine;

public readonly struct DamageData
{
    public readonly Health giver;
    public readonly int damage;

    public DamageData(Health giver, int damage)
    {
        this.giver = giver;
        this.damage = damage;
    }

    public static DamageData Create(Health giver, int damage)
    {
        DamageData result = new DamageData(giver, damage);
        return result;
    }
}

public readonly struct DamageResultData
{
    public readonly Health giver;
    public readonly int damage;
    public readonly int currentHealth;

    public DamageResultData(Health giver, int damage, int currntHealth)
    {
        this.giver = giver;
        this.damage = damage;
        this.currentHealth = currntHealth;
    }

    public static DamageResultData Create(Health giver, int damage, int currntHealth)
    {
        DamageResultData result = new DamageResultData(giver, damage, currntHealth);
        return result;
    }
}

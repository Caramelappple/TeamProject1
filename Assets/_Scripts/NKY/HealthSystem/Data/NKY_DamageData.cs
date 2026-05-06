using UnityEngine;

public readonly struct NKY_DamageData
{
    public readonly NKY_Health giver;
    public readonly int damage;

    public NKY_DamageData(NKY_Health giver, int damage)
    {
        this.giver = giver;
        this.damage = damage;
    }

    public static NKY_DamageData Create(NKY_Health giver, int damage)
    {
        NKY_DamageData result = new NKY_DamageData(giver, damage);
        return result;
    }
}

public readonly struct NKY_DamageResultData
{
    public readonly NKY_Health giver;
    public readonly int damage;
    public readonly int currentHealth;

    public NKY_DamageResultData(NKY_Health giver, int damage, int currntHealth)
    {
        this.giver = giver;
        this.damage = damage;
        this.currentHealth = currntHealth;
    }

    public static NKY_DamageResultData Create(NKY_Health giver, int damage, int currntHealth)
    {
        NKY_DamageResultData result = new NKY_DamageResultData(giver, damage, currntHealth);
        return result;
    }
}

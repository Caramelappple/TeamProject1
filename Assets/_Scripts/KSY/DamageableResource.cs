using System;
using UnityEngine;

public abstract class DamageableResource : MonoBehaviour
{
    protected bool isDestroyed
    {
        get
        {
            return value <= minValue;
        }
    }
    protected int maxValue;
    protected int minValue;
    protected int value;

    public Action<int> OnHit;
    public Action<int> OnDamage;

    public void Initialize(int maxValue, int minValue, int startValue)
    {
        this.maxValue = maxValue;
        this.minValue = minValue;
        this.value = startValue;
    }

    //public int GetValue() => _value;
    public int GetValue()
    {
        return value;
    }

    public virtual int GetDamage(int damageValue)
    {
        OnHit.Invoke(damageValue);

        if (isDestroyed) return minValue;

        value -= damageValue;
        OnDamage.Invoke(damageValue);

        return value;
    }

    public virtual int GetHeal(int healVaule)
    {
        if (value >= maxValue) return maxValue;
        value += healVaule;

        return value;
    }
}

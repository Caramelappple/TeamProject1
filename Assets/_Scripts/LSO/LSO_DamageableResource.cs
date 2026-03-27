using UnityEngine;

public abstract class DamageableResource : MonoBehaviour
{
    protected bool isDestryed;
    protected int maxValue;
    protected int minValue;
    protected int value;

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
        if (isDestryed) return minValue;

        value -= damageValue;
        if (value <= minValue) isDestryed = true;

        return value;
    }

    public virtual int GetHeal(int healVaule)
    {
        if (value >= maxValue) return maxValue;

        value += healVaule;

        return value;
    }
}
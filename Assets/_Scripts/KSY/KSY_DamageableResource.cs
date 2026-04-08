using System;
using UnityEngine;

public abstract class KSY_DamageableResource : MonoBehaviour
{
    protected int Value
    {
        get
        {
            return _value;
        }
        set
        {
            int calculValue = _value + value;
            _value = Mathf.Clamp(calculValue, minValue, maxValue);
        }
    }
    protected bool isDestroyed
    {
        get
        {
            return _value <= minValue;
        }
    }
    protected int maxValue;
    protected int minValue;

    private int _value;

    //Action<int> = int 자료형을 매개변수로 받는 메서드를 저장하는 delegate
    //public event Action<int,int,GameObject> OnHit; 
    public event Action<KSY_DamageEventArgs> OnDamage;
    public event Action OnHit;

    // 시그니처 -> 메서드들의 형식을 구분하는 특징 (중요한 점: 메서드들의 형식이고 메서드를 구분하는 게 아닙니다.)
    // 1. 반환값
    // 2. 매개변수

    public void Initialize(int maxValue, int minValue, int startValue)
    {
        this.maxValue = maxValue;
        this.minValue = minValue;
        this._value = startValue;
    }
    public int GetValue()
    {
        return _value;
    }

    public virtual int GetDamage(int damageValue, GameObject giver)
    {
        OnHit?.Invoke();

        if (isDestroyed) return minValue;

        KSY_DamageEventArgs args = new KSY_DamageEventArgs();

        _value -= damageValue;
        
        args.damage = damageValue;
        args.currentHealth = _value;

        OnDamage?.Invoke(args);

        return _value;
    }
}

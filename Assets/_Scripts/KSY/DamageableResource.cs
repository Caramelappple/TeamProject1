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

    //Action<int> = int 자료형을 매개변수로 받는 메서드를 저장하는 delegate
    //public event Action<int,int,GameObject> OnHit; 
    public event Action<DamageEventArgs> OnDamage;

    public delegate void HitHandler(int damage, GameObject giver);
    public event HitHandler OnHit;

    // 시그니처 -> 메서드들의 형식을 구분하는 특징 (중요한 점: 메서드들의 형식이고 메서드를 구분하는 게 아닙니다.)
    // 1. 반환값
    // 2. 매개변수

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

    public virtual int GetDamage(int damageValue, GameObject giver)
    {
        OnHit?.Invoke(damageValue, giver);

        if (isDestroyed) return minValue;

        // 대미지 연산
        value -= damageValue;

        // 대미지 이벤트 생성
        DamageEventArgs args = new DamageEventArgs();
        args.damage = damageValue;
        args.currentHealth = value;

        // 문제
        // 대미지를 받았을 때 현재 체력과 받은 대미지 출력하기.

        // 대미지 이벤트 호출
        OnDamage?.Invoke(args);

        return value;
    }

    public virtual int GetHeal(int healVaule)
    {
        if (value >= maxValue) return maxValue;
        value += healVaule;

        return value;
    }
}

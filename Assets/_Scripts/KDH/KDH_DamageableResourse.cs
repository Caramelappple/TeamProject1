using System;
using UnityEngine;

public abstract class KDH_DamageAbleResorce : MonoBehaviour
{
    protected int Value
    {
        get
        {
            return _value;
        }
        set
        {
            int calcuValue = _value + value;

            _value = Mathf.Clamp(calcuValue, minValue, maxValue);
        }
    }

    private bool _isDestroyed;
    protected bool isDestroyed
    {
        get
        {
            return _value <= minValue;
        }
    }
    //private bool _isDestroy;
    //public bool GetIsDestroy ()
    //{
    //    return _isDestroy;
    //}   

    //public void SetIsDestroyed (bool value)
    //{
    //    _isDestroy = value;
    //}

    //protected bool isDestory //protected 자식만 사용할 수 있다.
    protected int maxValue;
    protected int minValue;
    private int _value;

    public event Action OnHit; //"event"는 이 스크립트에서만 쓸수 있으니깐 디버깅하기에 좋음.
    public event Action<KDH_DamageArgs> OnDamaged; //"event"를 쓰는 순간 '='로 호출하지 못하고 외부에서 건딜 수 없음

    public delegate void HitHandler(int damage, GameObject giver);

    // 시그니처 -> 메서드들의 형식을 구분하는 특징( 중요한점: 메서드들의 형식이고 메서드를 구분하는 게 아니다.)
    // 1. 반환값
    // 2. 매개변수야
    public void Initalize(int _maxValue, int _minValue, int startValue)
    {
        this.maxValue = _maxValue;
        this.minValue = _minValue;
        this._value = startValue;
    }

    public int GetValue()
    {
        return _value;
    }

    public virtual int GetDamage(int damageValue, GameObject giver)
    {
            OnHit?.Invoke(); //'?'는 Nullable이다 Nullable은 Null값을 담을 수 있도록 허용한다.

        if (isDestroyed) return minValue;

        //대미지 연산
        _value -= damageValue;

        Mathf.Clamp(_value, minValue, maxValue);

        //대미지 이벤트 생성
        KDH_DamageArgs args = new KDH_DamageArgs();
        args.damage = damageValue;
        args.currentHealth = _value;

        //대미지 이벤트 호출
        OnDamaged?.Invoke(args);

        return _value;
    }
}
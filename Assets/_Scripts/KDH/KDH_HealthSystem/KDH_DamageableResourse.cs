using System;
using Unity.VisualScripting;
using UnityEngine;

//protected = 상속을 받은 얘만 가능
public abstract class KDH_DamageAbleResorce : MonoBehaviour
{
    [SerializeField] private KDH_HealthSO _data;

    [field : SerializeField] public int MaxValue {  get; private set; }
    [field : SerializeField] public  int MinValue {  get; private set; }
    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = Mathf.Clamp(value, MinValue, MaxValue);
        }
    }

    public bool _isDestroyed
    {
        get
        {
            return _value <= MinValue;
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
    private int _value;

    public bool IsDamageable { get; private set; }
    public event Action<KDH_DamageResultData> OnDamaged; //"event"를 쓰는 순간 '='로 호출하지 못하고 외부에서 건딜 수 없음
    public event Action<KDH_DamageData> OnHit; //"event"는 이 스크립트에서만 쓸수 있으니깐 디버깅하기에 좋음.

    public delegate void HitHandler(int damage, GameObject giver);
    public void SetDamageable(bool value)
    {
        IsDamageable = value;
    }

    // 시그니처 -> 메서드들의 형식을 구분하는 특징( 중요한점: 메서드들의 형식이고 메서드를 구분하는 게 아니다.)
    // 1. 반환값
    // 2. 매개변수야

    private void OnValidate()
    {
        Initalize();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
    }
    public void Initalize()
    {
        MaxValue = _data.MaxValue;
        MinValue = _data.MinValue;
        _value = _data.StartValue;
    }

    public int GetValue()
    {
        return _value;
    }

    public virtual void GetDamage(int damageValue, Entity giver)
    {
        KDH_DamageData data = KDH_DamageData.Create(giver, damageValue);
        OnHit?.Invoke(data); //'?'는 Nullable이다 Nullable은 Null값을 담을 수 있도록 허용한다.

        if (_isDestroyed || IsDamageable) return;

        _value -= damageValue;

        //대미지 이벤트 호출
        KDH_DamageResultData resultData = KDH_DamageResultData.Create(giver, damageValue, Value);
        OnDamaged?.Invoke(resultData);
    }
}
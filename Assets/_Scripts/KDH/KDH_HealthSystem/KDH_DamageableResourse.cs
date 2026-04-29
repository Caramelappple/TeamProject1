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

    private int _value;

    public bool IsDamageable { get; private set; }
    public event Action<KDH_DamageResultData> OnDamaged; //"event"를 쓰는 순간 '='로 호출하지 못하고 외부에서 건딜 수 없음
    public event Action<KDH_DamageData> OnHit; //"event"는 이 스크립트에서만 쓸수 있으니깐 디버깅하기에 좋음.

    public delegate void HitHandler(int damage, GameObject giver);
    public void SetDamageable(bool value)
    {
        IsDamageable = value;
    }

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

    public virtual void GetDamage(int damageValue, KDH_Entity giver)
    {

        KDH_DamageData data = KDH_DamageData.Create(giver, damageValue);
        OnHit?.Invoke(data);

        if (_isDestroyed || IsDamageable) return;

        _value -= damageValue;

        //대미지 이벤트 호출
        KDH_DamageResultData resultData = KDH_DamageResultData.Create(giver, damageValue, Value);
        OnDamaged?.Invoke(resultData);
    }
}
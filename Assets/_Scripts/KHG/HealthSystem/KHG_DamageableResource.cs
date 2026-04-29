using System;
using UnityEngine;

public abstract class KHG_DamageableResource : MonoBehaviour
{
    [SerializeField] private DamageableResourceSo _data;
    [field : SerializeField] public int MaxValue { get; private set; }
    [field: SerializeField] public int MinValue { get; private set; }
    public bool IsDamageable { get; private set; }
   
    public event Action<KHG_DamageResultData> OnDamage;
    public event Action<KHG_DamageData> OnHit;

    [field: SerializeField] private int _value;
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
    public bool isDestroyed
    {
        get
        {
            return _value <= MinValue;
        }
    }
   

   // private void Awake()
   // {
        //Initialize();
        
   // }
    private void OnValidate()
    {
        Initialize();
    }
    public void Initialize()
    {
        this.MaxValue = _data.MaxValue;
        this.MinValue = _data.MinValue;
        this._value = _data.StartValue;
    }
    public int GetValue()
    {
        return _value;
    }
    public void SetDamageable(bool value)
    {
        IsDamageable = value;
    }
    public void GetDamage(int damageValue, KHG_Entity giver)
    {
        KHG_DamageData damageData = KHG_DamageData.Create(giver, damageValue);
        OnHit?.Invoke(damageData);

        if (isDestroyed || !IsDamageable) return;

        _value -= damageValue;

        KHG_DamageResultData resultData = KHG_DamageResultData.Create(giver, damageValue);
        OnDamage?.Invoke(resultData);

    }
}

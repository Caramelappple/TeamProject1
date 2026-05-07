//이거 사용
using System;
using Unity.VisualScripting;
using UnityEngine;

public class NKY_DamageableResources : MonoBehaviour
{
    [SerializeField] private NKY_DamageableResourceSO _data;
    [field : SerializeField] public int MaxValue { get; private set; }
    [field: SerializeField]  public int MinValue { get; private set; }
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
    public bool IsDestroyed
    {
        get
        {
            return _value <= MinValue;
        }
    }

    public bool IsDamageable { get; private set; } = true;
    
    public event Action<NKY_DamageResultData> OnDamage;
    public event Action<NKY_DamageData> OnHit;

    [SerializeField] private int _value;
    
    protected virtual void Awake()
    {
        Initialize();
        OnDamage += (data) => Debug.Log($"<color=red>{data.giver}로부터 {data.damage}만큼 대미지를 받았습니다!</color>");
    }
    
    public void Initialize()
    {
        MaxValue = _data.maxValue;
        MinValue = _data.minValue;
        _value = _data.startValue;
    }
    
    public int GetValue()
    {
        return _value;
    }

    public void SetDamageable(bool value)
    {
        IsDamageable = value;
    }
    
    
    public virtual void GetDamage(NKY_DamageData data)
    {
        NKY_DamageData damagedata = data;
        OnHit?.Invoke(damagedata);
        int damage = data.damage;
        NKY_Health giver = data.giver;

        if(IsDestroyed || !IsDamageable) return;
        int listValue = Value;
        Value -= damage;
        int calcValue = Value;
        bool hasDamage = listValue > calcValue;

        if (hasDamage)
        {
            NKY_DamageResultData resultData = NKY_DamageResultData.Create(giver, damage, Value);
            OnDamage?.Invoke(resultData);
        }
    }
}

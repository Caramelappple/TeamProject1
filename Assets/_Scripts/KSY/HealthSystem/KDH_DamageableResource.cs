using Assets._Scripts.KSY;
using KSY.HealthSystem;
using System;
using UnityEngine;

public class KSY_DamageableResourceq : MonoBehaviour
{
    [SerializeField] private KSY_DamageableResourceSO _data;
    [field: SerializeField] public int MaxValue { get; private set; }
    [field: SerializeField] public int MinValue { get; private set; }

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

    public event Action<KDH_DamageResultData> OnDamage;
    public event Action<KDH_DamageData> OnHit;

    [SerializeField] private int _value;

    protected virtual void Awake()
    {
        Initialize();
        OnDamage += (data) => Debug.Log($"<color=red>{data.giver}로부터 {data.damage}만큼 대미지를 받았습니다!</color>");
    }
    public void Initialize()
    {
        MaxValue = _data.MaxValue;
        MinValue = _data.MinValue;
        Value = _data.StartValue;
    }
    public int GetValue()
    {
        return _value;
    }
    public void SetDamageable(bool value)
    {
        IsDamageable = value;
    }
    public void GetDamage(KDH_DamageData data)
    {
        int damage = data.damage;
        KDH_Entity giver = data.giver;

        OnHit?.Invoke(data);

        if (IsDestroyed || !IsDamageable) return;

        _value -= damage;

        KDH_DamageResultData resultData = KDH_DamageResultData.Create(giver, damage, Value);
        OnDamage?.Invoke(resultData);
    }

    [ContextMenu("Damage")]
    public void GetDamage()
    {
        KDH_DamageData data = KDH_DamageData.Create(null, 1);
        int damage = data.damage;
        KDH_Entity giver = data.giver;

        OnHit?.Invoke(data);

        if (IsDestroyed || !IsDamageable) return;

        _value -= damage;

        KDH_DamageResultData resultData = KDH_DamageResultData.Create(giver, damage, Value);
        OnDamage?.Invoke(resultData);
    }
}

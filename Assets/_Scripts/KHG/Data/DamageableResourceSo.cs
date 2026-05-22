using UnityEngine;

[CreateAssetMenu(fileName = "HealthSo", menuName = "Scriptable Objects/HealthSo")]
public class DamageableResourceSo : ScriptableObject
{
    [field: SerializeField] public int MaxValue { get; private set; }
    [field: SerializeField] public int MinValue { get; private set; }
    [field: SerializeField] public int StartValue { get; private set; }
}

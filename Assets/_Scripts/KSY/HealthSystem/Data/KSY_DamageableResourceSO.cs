using UnityEngine;

namespace KSY.HealthSystem
{
    [CreateAssetMenu(fileName = "KSY_DamageableResourceSO", menuName = "SO/KSY_DamageableResourceSO", order = 0)]
    public class KSY_DamageableResourceSO : ScriptableObject
    {
        [field: SerializeField] public int MaxValue { get; private set; }
        [field: SerializeField] public int MinValue { get; private set; }
        [field: SerializeField] public int StartValue { get; private set; }
    }
}
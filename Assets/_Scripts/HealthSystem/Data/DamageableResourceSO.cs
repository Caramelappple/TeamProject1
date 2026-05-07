//이거 사용
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "Damageable Resource", menuName = "SO/Damageable Resource")]
public class DamageableResourceSO : ScriptableObject
{
    [field: SerializeField] public int maxValue { get; private set; }
    [field: SerializeField] public int minValue { get; private set; }
    [field: SerializeField] public int startValue { get; private set; }
}

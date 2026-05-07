//이거 사용
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "NKY_DamageableResourceSO", menuName = "Scriptable Objects/NKY_DamageableResourceSO")]
public class DamageableResourceSO : ScriptableObject
{
    [field: SerializeField] public int maxValue { get; private set; }
    [field: SerializeField] public int minValue { get; private set; }
    [field: SerializeField] public int startValue { get; private set; }
}

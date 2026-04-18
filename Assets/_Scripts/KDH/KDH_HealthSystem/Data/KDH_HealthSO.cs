using UnityEngine;

[CreateAssetMenu(fileName = "KDH_HealthSO", menuName = "Scriptable Objects/KDH_HealthSO")]
public class KDH_HealthSO : ScriptableObject
{
    [field: SerializeField] public int MaxValue { get; private set; }
    [field: SerializeField] public int MinValue { get; private set; }
    [field: SerializeField] public int StartValue { get; private set; }
}
                                                                                                                                                                                                                                                                                                                                    
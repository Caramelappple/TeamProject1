using TMPro;
using UnityEngine;

public class KDH_BossNameSingle : MonoBehaviour
{
    public static KDH_BossNameSingle instance;

    private void Awake()
    {
        instance = this;
    }

    [field: SerializeField] public TextMeshProUGUI title { get; private set; }
}

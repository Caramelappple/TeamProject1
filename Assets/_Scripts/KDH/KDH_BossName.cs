using TMPro;
using UnityEngine;

public class KDH_BossName : MonoBehaviour
{
    public string bossName;
    [SerializeField] private TextMeshProUGUI UI;

    private void Start()
    {
        KDH_BossNameSingle.instance.title.text = bossName;
    }
}

using TMPro;
using UnityEngine;

public class KDH_CurrentHPText : MonoBehaviour
{
    public static KDH_CurrentHPText Instance;

    public TextMeshProUGUI currentHpText;

    private KDH_HealthBarUI _health;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void UpdateHPText(float Hp)
    {
        currentHpText.text = "Hp :" + Hp.ToString();
    }
}
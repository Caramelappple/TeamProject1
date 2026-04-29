using TMPro;
using UnityEngine;

public class KDH_CurrentHPText : MonoBehaviour
{
    public static KDH_CurrentHPText Instance;

    public TextMeshProUGUI currentHpText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void UpdateHPText(int Hp)
    {
        currentHpText.text = "Hp :" + Hp.ToString();
    }
}
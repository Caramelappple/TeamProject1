using UnityEngine;

public class KDH_Disable : MonoBehaviour
{
    [SerializeField] private GameObject skillSelectUI;

    public void Disable()
    {
        skillSelectUI.SetActive(false);
    }
}

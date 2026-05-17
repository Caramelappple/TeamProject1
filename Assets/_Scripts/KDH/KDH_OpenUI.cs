using UnityEngine;

public class KDH_OpenUI : MonoBehaviour
{
    [SerializeField] private GameObject UI1;
    [SerializeField] private GameObject UI2;

    private void Start()
    {
        UI1.SetActive(false);
    }

    public void OnUI()
    {
        UI1.SetActive(true);
        UI2.SetActive(false);
        gameObject.SetActive(true);
    }
}

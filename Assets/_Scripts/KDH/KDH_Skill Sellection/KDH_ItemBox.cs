using UnityEngine;

public class KDH_ItemBox : MonoBehaviour
{
    [SerializeField] private GameObject selectSkillUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            selectSkillUI.SetActive(true);
        }
    }
}

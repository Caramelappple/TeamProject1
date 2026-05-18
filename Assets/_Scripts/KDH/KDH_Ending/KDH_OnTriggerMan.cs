using UnityEngine;

public class KDH_OnTriggerMan : MonoBehaviour
{
    [SerializeField] private GameObject endingBox;

    private void Start()
    {
        endingBox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            endingBox.SetActive(true);
        }
    }
}

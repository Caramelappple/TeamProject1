using UnityEngine;

public class KDH_OnTriggerMan : MonoBehaviour
{
    private GameObject endingBox;

    private void Start()
    {
        endingBox = NKY_GameManager.instance.EndUI;
        endingBox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(endingBox);

        if (collision.CompareTag("Player"))
        {
            endingBox.SetActive(true);
        }
    }
}

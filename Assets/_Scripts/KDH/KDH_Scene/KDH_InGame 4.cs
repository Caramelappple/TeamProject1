using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_InGame4 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(4);
        }
    }
}

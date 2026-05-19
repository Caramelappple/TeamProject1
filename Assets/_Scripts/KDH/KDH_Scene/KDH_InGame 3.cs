using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_InGame3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(3);
        }
    }
}

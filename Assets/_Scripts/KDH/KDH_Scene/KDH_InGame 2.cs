using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_InGame2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(3);
            NKY_GameManager.instance.player.transform.position = new Vector3(0f, 0f, 0f); ;
        }
    }
}

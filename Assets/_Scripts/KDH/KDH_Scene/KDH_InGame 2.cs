using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_InGame2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (NKY_GameManager.instance != null)
            {
                NKY_GameManager.instance.SaveCurrentStatus();
            }

            if (KDH_SceneFader.Instance != null)
            {
                KDH_SceneFader.Instance.FadeToScene("KHG_map2");
            }
        }
    }
}
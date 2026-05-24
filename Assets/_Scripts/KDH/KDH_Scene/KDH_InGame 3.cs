using UnityEngine;

public class KDH_InGame3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (KDH_SceneFader.Instance != null)
            {
                KDH_SceneFader.Instance.FadeToScene("KHG_map3");
            }
        }
    }
}
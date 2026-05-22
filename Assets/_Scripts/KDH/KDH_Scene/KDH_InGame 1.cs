using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_InGame1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            KDH_SceneFader.Instance.FadeToScene("InGame");
            NKY_GameManager.instance.player.transform.position = new Vector3(0f, 0f, 0f); ;
        }
    }
}

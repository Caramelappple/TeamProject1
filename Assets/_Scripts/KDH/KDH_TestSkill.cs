using Unity.VisualScripting;
using UnityEngine;

public class KDH_TestSkill : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private SpriteRenderer sprite;
    private Color color = Color.red;

    private void OnEnable()
    {
        gameObject.SetActive (true);
        sprite.color = color;
    }
}

using TMPro;
using UnityEngine;

public class KDH_HealAnim : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private TMP_Text text;
    private Color alpha;

    void Start()
    {
        text = GetComponent<TMP_Text>();

        text.color = Color.green;
        alpha = text.color;

        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;

        Destroy(gameObject, 2.0f);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));

        if (text != null)
        {
            alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
            text.color = alpha;
        }
    }

    public void Setup(int heal)
    {
        GetComponent<TMP_Text>().text = "+" + heal.ToString();
    }
}
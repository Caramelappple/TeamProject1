using TMPro;
using UnityEngine;

public class KDH_DamageAnim : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    private TMP_Text text;
    private Color alpha;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        alpha = text.color;

        moveSpeed = 3.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // 위치 이동
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));

        // 중력 효과
        moveSpeed -= 5.0f * Time.deltaTime;

        // 투명도 감소
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

   
    public void Setup(int damage)
    {
        GetComponent<TMP_Text>().text = damage.ToString();
    }
}

using UnityEngine;

public class KDH_BackgroundMoving : MonoBehaviour
{
    public float speed = 1f;
    public float height = 5f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * height;
        transform.localPosition = startPos + new Vector3(0, y, 0);
    }
}

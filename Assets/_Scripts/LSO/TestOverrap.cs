using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform pos;
    [SerializeField] private LayerMask _enemyLayer;

    private readonly Vector2 _boxSize = new Vector2(1, 1);
    private float _checkInterval = 0.1f;
    private float _timer;

    void Start()
    {
        if (pos == null)
        {
            Debug.LogError("pos Transform이 할당되지 않았습니다!");
            return;
        }
        pos.position = Vector2.zero;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < _checkInterval) return;

        _timer = 0f;

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(
            pos.position, _boxSize, 0, _enemyLayer
        );

        foreach (Collider2D collision in collider2Ds)
        {
            if (collision.CompareTag("Enemy"))
                Debug.Log("충돌: " + collision.name);
        }
    }

    // 에디터에서 박스 범위 시각화
    void OnDrawGizmos()
    {
        if (pos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, _boxSize);
    }
}
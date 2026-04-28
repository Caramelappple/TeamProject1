using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public static IndicatorManager Instance { get; private set; }

    [SerializeField] private GameObject _indicatorPrefab;
    [SerializeField] private int _poolSize = 10;

    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_indicatorPrefab, transform);
            obj.SetActive(false);

            _pool.Enqueue(obj);
        }
    }

    public void ShowIndicator(Collider2D collider, float duration)
    {
        ShowIndicator(collider, collider.bounds.center, duration);
    }

    //public IEnumerator ShowIndicatorCoroutine()
    //{

    //}
    public void ShowIndicator(Collider2D collider, Vector2 position , float duration)
    {
        Vector2 size = collider.bounds.size;
        if (_pool.Count == 0) return;

        GameObject obj = _pool.Dequeue();

        // 위치와 크기 설정
        obj.transform.position = position;
        obj.transform.localScale = size;
        obj.SetActive(true);

        StartCoroutine(HideIndicator(obj, duration));
    }

    private IEnumerator HideIndicator(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}

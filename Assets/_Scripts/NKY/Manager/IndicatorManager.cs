using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.NKY.Manager
{
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

        public void ShowIndicator(Collider2D col, float duration)
        {
            ShowIndicator(col, col.bounds.center, duration);
        }

        //public IEnumerator ShowIndicatorCoroutine()
        //{

        //}

        //public void ShowIndicator(Collider2D col, Vector3 position, float duration)
        //{
            
        //}
        public void ShowIndicator(Collider2D col, Vector2 position , float duration)
        {
            Quaternion rotate = col.transform.rotation;
            Vector2 size = col.bounds.size;
            if (_pool.Count == 0) return;

            GameObject obj = _pool.Dequeue();

            // 위치와 크기 설정
            obj.transform.rotation = rotate;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.NKY.Manager
{
    public class IndicatorManager : MonoBehaviour
    {
        public static IndicatorManager Instance { get; private set; }

        [SerializeField] private GameObject[] _indicatorPrefab;
        [SerializeField] private int _poolSize = 5;

        private Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();
        //private Queue<GameObject>[] _pool = new Queue<GameObject>();

        private void Awake()
        {
            Instance = this;
            foreach (var Prefab in _indicatorPrefab)
            {
                string prefabName = Prefab.name;
                if (!_pool.ContainsKey(prefabName))
                {
                    _pool[prefabName] = new Queue<GameObject>();
                }
                for (int i = 0; i < _poolSize; i++)
                {
                    GameObject obj = Instantiate(Prefab, transform);
                    obj.SetActive(false);

                    _pool[prefabName].Enqueue(obj);
                }
            }
        }

        public void ShowIndicator(Collider2D col, float duration)
        {
            Vector2 exactPos = col.transform.TransformPoint(col.offset);
            ShowIndicator(col, exactPos, duration);
        }
        public void ShowIndicator(Collider2D col, Vector2 position , float duration)
        {
            /*Quaternion rotate = col.transform.rotation;
            Vector2 size = col.bounds.size;
            if (_pool.Count == 0) return;

            GameObject obj = _pool.Dequeue();

            // 위치와 크기 설정
            obj.transform.rotation = rotate;
            obj.transform.position = position;
            obj.transform.localScale = size;
            obj.SetActive(true);

            StartCoroutine(HideIndicator(obj, duration));*/

            Quaternion rotate = col.transform.rotation;
            
            Vector2 realSize = Vector2.one;

            string targetPrefabName = _indicatorPrefab[0].name;
            
            if (col is BoxCollider2D box)
            {
                realSize = box.size;
                targetPrefabName = _indicatorPrefab[0].name;
            }
            else if (col is CircleCollider2D circle)
            {
                float diameter = circle.radius * 2f;
                realSize = new Vector2(diameter, diameter);
                targetPrefabName = _indicatorPrefab[1].name;
            }
            else if(col is CapsuleCollider2D capsule)
            {
                realSize = capsule.size;
                targetPrefabName = _indicatorPrefab[2].name;
            }
            
            if (string.IsNullOrEmpty(targetPrefabName) || !_pool.ContainsKey(targetPrefabName) || _pool[targetPrefabName].Count == 0) 
                return;

            realSize.x *= col.transform.lossyScale.x;
            realSize.y *= col.transform.lossyScale.y;
            
            GameObject obj = _pool[targetPrefabName].Dequeue();

            obj.transform.rotation = rotate;
            obj.transform.position = position;
            
            obj.transform.localScale = new Vector3(realSize.x, realSize.y, 1f);
    
            obj.SetActive(true);

            StartCoroutine(HideIndicator(obj, duration, targetPrefabName));
        }

        private IEnumerator HideIndicator(GameObject obj, float delay, string name)
        {
            yield return new WaitForSeconds(delay);
            obj.SetActive(false);
            _pool[name].Enqueue(obj);
        }
    }
}

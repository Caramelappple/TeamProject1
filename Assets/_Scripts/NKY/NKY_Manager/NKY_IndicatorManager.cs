using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.NKY.Manager
{
    public class NKY_IndicatorManager : MonoBehaviour
    {
        public static NKY_IndicatorManager Instance { get; private set; }

    // ???? ?? ??? ?? Enum ??? ?????.
    public enum IndicatorType { Box = 0, Circle = 1, Capsule = 2, Custom = 3 }

    [SerializeField] private GameObject[] _indicatorPrefab;
    [SerializeField] private int _poolSize = 10;

    private Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        foreach (var prefab in _indicatorPrefab)
        {
            string prefabName = prefab.name;
            if (!_pool.ContainsKey(prefabName))
                _pool[prefabName] = new Queue<GameObject>();

            for (int i = 0; i < _poolSize; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                _pool[prefabName].Enqueue(obj);
            }
        }
    }

    #region [?? 1] ???? ?? (?? ?? ??)
    public void ShowIndicator(Collider2D col, Vector2 position, float duration)
    {
        // ????? ?? ?? ?? ??
        Vector2 exactPos = position;
        
        Vector2 realSize = Vector2.one;
        IndicatorType type = IndicatorType.Box;

        // ???? ??? ?? ??? ? ?? ??
        if (col is BoxCollider2D box)
        {
            realSize = box.size;
            type = IndicatorType.Box;
        }
        else if (col is CircleCollider2D circle)
        {
            realSize = new Vector2(circle.radius * 2f, circle.radius * 2f);
            type = IndicatorType.Circle;
        }
        else if (col is CapsuleCollider2D capsule)
        {
            realSize = capsule.size;
            type = IndicatorType.Capsule;
        }
        else
        {
            type = IndicatorType.Custom;
        }

        // ??? ? ??
        realSize.x *= col.transform.lossyScale.x;
        realSize.y *= col.transform.lossyScale.y;

        // ??? ??? ??(Polygon ?) ?? ??
        Mesh customMesh = (type == IndicatorType.Custom) ? col.CreateMesh(true, true) : null;

        // ?? ?? ?? ??
        ExecuteShow(type, exactPos, col.transform.rotation, realSize, duration, customMesh);
    }
    #endregion

    #region [?? 2] ?? ?? (???? ??? ?? ??)
    /// <summary>
    /// ???? ??, ??, ??? ???? ?????? ?????.
    /// </summary>
    public void ShowIndicator(IndicatorType type, Vector2 position, Vector2 size, float duration, float angle = 0f)
    {
        ExecuteShow(type, position, Quaternion.Euler(0, 0, angle), size, duration);
    }
    #endregion

    // ?? ????? ??? ???? ?? ??
    private void ExecuteShow(IndicatorType type, Vector2 pos, Quaternion rot, Vector2 size, float duration, Mesh customMesh = null)
    {
        int index = (int)type;
        if (index >= _indicatorPrefab.Length) return;

        string targetName = _indicatorPrefab[index].name;

        if (!_pool.ContainsKey(targetName) || _pool[targetName].Count == 0)
        {
            // ?? ???? ?? ?? (????)
            GameObject newObj = Instantiate(_indicatorPrefab[index], transform);
            _pool[targetName].Enqueue(newObj);
        }

        GameObject obj = _pool[targetName].Dequeue();
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        // ?? ??
        if (type == IndicatorType.Custom && customMesh != null)
        {
            if (obj.TryGetComponent<MeshFilter>(out var mf)) mf.mesh = customMesh;
        }
        else
        {
            obj.transform.localScale = new Vector3(size.x, size.y, 1f);
        }

        obj.SetActive(true);
        StartCoroutine(HideIndicator(obj, duration, targetName));
    }

    private IEnumerator HideIndicator(GameObject obj, float delay, string poolName)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        _pool[poolName].Enqueue(obj);
    }
    }
}

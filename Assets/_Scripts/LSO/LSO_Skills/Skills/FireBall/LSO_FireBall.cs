    using System.Collections;
using UnityEngine;

public class LSO_FireBall : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    
    [SerializeField]private GameObject explosionPrefab;
    
    private Rigidbody2D _rigid;
    
    private float _liveTime = 2.2f;
    private float _speed = 5f;
    private Vector2 _lastDir;
    private bool _canExplode = true;

    public void Init(Vector3 dir)
    {
        _lastDir = dir;
    }
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rigid.linearVelocity = _lastDir * _speed;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        if (!_canExplode) yield break;
        yield return new WaitForSeconds(_liveTime);
        LSO_SoundManager.Instance.SfxPlay(clip);
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _canExplode = false;
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    
}

using System.Collections;
using UnityEngine;

public class LSO_CameraMovement : MonoBehaviour
{
    [SerializeField]private Transform target;
    private Transform _originTarget;
    [SerializeField]private Transform[] bossTargets;
    
    [SerializeField]private float speed = 1.2f;
    private float _originSpeed;

    public bool test;
    [SerializeField]private bool last;
    private bool _isReturning;

    private void Start()
    {
        _originTarget = target;
        _originSpeed = speed;
    }
    private void FixedUpdate()
    {
        if (test)
        {
            if (bossTargets == null || bossTargets.Length == 0 || bossTargets[0] == null) return;

            target = bossTargets[0];
            speed = 5;
            last = true;
            _isReturning = false;
        }
        else if (last && !test && !_isReturning)
        {
            last = false;
            _isReturning = true;
            target = _originTarget;
            speed = 50;
            StartCoroutine(CameraWait());
        }
    }

    private void LateUpdate()
    {
        Vector3 shakeOffset = CameraShake.instance != null
            ? CameraShake.instance.ShakeOffset
            : Vector3.zero;

        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        transform.position = new Vector3(
            transform.position.x + shakeOffset.x,
            transform.position.y + shakeOffset.y,
            -10f
        );
    }

    private IEnumerator CameraWait()
    {
        yield return new WaitUntil(() => target == null || Vector3.Distance(transform.position, target.position) < 0.1f);

        speed = _originSpeed;
        _isReturning = false;
    }
    
    CameraShake _cameraShake;
}

using System.Collections;
using UnityEngine;

public class LSO_CameraMovement : MonoBehaviour
{
    [SerializeField]private Transform target;
    private Transform _originTarget;
    [SerializeField]private Transform[] bossTargets;
    
    [SerializeField]private float speed = 1.2f;
    private float _originSpeed;

    [SerializeField]private bool test;
    [SerializeField]private bool last;
    private bool _isReturning;

    private void Start()
    {
        _originTarget = target;
        _originSpeed = speed;
    }
    private void FixedUpdate()
    {
        if (test)//보스에게 카메라 이동 테스트용, 나중에 수정 필요
        {
            target = bossTargets[0];
            speed = 5;
            last = true;
            _isReturning = false;
        }
        else if (last && !test && !_isReturning) // 중복 실행 방지, 플레이어가 보스에게 돌아오지 않은 상태에서만 실행
        {
            last = false;
            _isReturning = true;
            target = _originTarget;
            speed = 50;// 플레이어에게 빠르게 돌아오기 위해 속도 증가
            StartCoroutine(CameraWait());
        }
    }

    private IEnumerator CameraWait()//카메라가 플레이어에게 돌아올 때까지 기다리는 코루틴
    {
        yield return new WaitUntil(() =>
            Vector3.Distance(transform.position, target.position) < 0.1f);

        speed = _originSpeed;// 원래 속도로 복귀
        _isReturning = false;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        transform.position = new Vector3(
            transform.position.x + CameraShake.instance.ShakeOffset.x,
            transform.position.y + CameraShake.instance.ShakeOffset.y,
            -10f
        );
    }
    
    CameraShake _cameraShake;
}

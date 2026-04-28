using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LSO_CameraMovement : MonoBehaviour
{
    [SerializeField]private Transform target;
    private Transform originTarget;
    [SerializeField]private Transform[] bossTargets;
    
    [SerializeField]private float speed = 1f;
    private float originSpeed;

    [SerializeField]private bool test;
    [SerializeField]private bool last;
    private bool isReturning = false;

    private void Start()
    {
        originTarget = target;
        originSpeed = speed;
    }
    private void FixedUpdate()
    {
        if (test)//보스에게 카메라 이동 테스트용, 나중에 수정 필요
        {
            target = bossTargets[0];
            speed = 2;
            last = true;
            isReturning = false;
        }
        else if (last && !test && !isReturning) // 중복 실행 방지, 플레이어가 보스에게 돌아오지 않은 상태에서만 실행
        {
            last = false;
            isReturning = true;
            target = originTarget;
            speed = 50;// 플레이어에게 빠르게 돌아오기 위해 속도 증가
            StartCoroutine(CameraWait());
        }
    }

    private IEnumerator CameraWait()//카메라가 플레이어에게 돌아올 때까지 기다리는 코루틴
    {
        yield return new WaitUntil(() =>
            Vector3.Distance(transform.position, target.position) < 0.1f);

        speed = originSpeed;// 원래 속도로 복귀
        isReturning = false;
    }

}

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Dashskill : MonoBehaviour, LSO_ISkill
{
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.8f;
    [SerializeField] private float dashCooltime = 0.1f;

    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private TrailRenderer tr;

    // KHG 대신 LSO_PlayerMovement로 변경
    private LSO_PlayerMovement playerMovement; 
    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        // 컴포넌트도 LSO로 가져옵니다.
        playerMovement = GetComponent<LSO_PlayerMovement>();
    }

    private void Update()
    {
        // 안전장치: playerMovement가 없을 경우 실행 안 함
        if (playerMovement == null) return;

        // 기존 F키 입력으로도 작동하게 두고 싶다면 유지 (LSO 시스템과 겹치지 않는지 확인 필요)
        if (Keyboard.current.fKey.wasPressedThisFrame && canDash)
        {
            Vector2 dashDir = playerMovement.GetLastDir();
            if (dashDir != Vector2.zero)
            {
                StartCoroutine(DashRoutine(dashDir));
            }
        }
    }

    private IEnumerator DashRoutine(Vector2 dir)
    {
        canDash = false;
        isDashing = true;

        // LSO에는 SetDashing이 없으므로 SetMove(false)로 움직임을 제어합니다.
        if (playerMovement != null) playerMovement.SetMove(false);
        
        rigid.linearVelocity = dir.normalized * dashSpeed;

        if (tr != null) tr.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        if (tr != null) tr.emitting = false;
       
        isDashing = false;
        
        // 대시가 끝나면 다시 움직일 수 있게 켭니다.
        if (playerMovement != null) playerMovement.SetMove(true);

        yield return new WaitForSeconds(dashCooltime);
        canDash = true;
    }

    // LSO 시스템인 인터페이스를 통해 실행될 때 호출되는 함수
    public void UseSkill(GameObject player)
    {
        if (playerMovement == null) playerMovement = player.GetComponent<LSO_PlayerMovement>();
        
        if (playerMovement != null && canDash)
        {
            Vector2 dashDir = playerMovement.GetLastDir();
            if (dashDir != Vector2.zero)
            {
                StartCoroutine(DashRoutine(dashDir));
            }
        }
    }

    public IEnumerator CoolTime(float time)
    {
        throw new System.NotImplementedException();
    }

    public bool IsDashing => isDashing; 
}
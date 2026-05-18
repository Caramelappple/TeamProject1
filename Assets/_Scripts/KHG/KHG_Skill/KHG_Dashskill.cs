using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class KHG_Dashskill : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.8f;
    [SerializeField] private float dashCooltime = 0.1f;

    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private TrailRenderer tr;

    private LSO_PlayerMovement playerMovement; 
    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<LSO_PlayerMovement>();
    }

    private void Update()
    {
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

    public bool IsDashing => isDashing; 
}
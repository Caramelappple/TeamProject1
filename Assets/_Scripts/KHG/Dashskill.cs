using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Dashskill : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.8f;
    [SerializeField] private float dashCooltime = 0.1f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private TrailRenderer tr;

    private PlayerMovement playerMovement; 
    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
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

        playerMovement.SetDashing(true);

        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f;

        rigid.linearVelocity = dir.normalized * dashSpeed;

        if (tr != null) tr.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        if (tr != null) tr.emitting = false;
        rigid.gravityScale = originalGravity;

        isDashing = false;
        playerMovement.SetDashing(false);

        yield return new WaitForSeconds(dashCooltime);
        canDash = true;
    }

    public bool IsDashing => isDashing; 
}



using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Serialization;

public class KHG_DashSkill : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.8f;
     [SerializeField] private float dashCoolTime = 0.1f;

    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private TrailRenderer tr;

    private LSO_PlayerMovement _playerMovement; 
    private bool _isDashing;
    private bool _canDash = true;

    private void Awake()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<LSO_PlayerMovement>();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && _canDash)
        {
            Vector2 dashDir = _playerMovement.GetLastDir();
            if (dashDir != Vector2.zero)
            {
                StartCoroutine(DashRoutine(dashDir));
            }
        }
    }

    private IEnumerator DashRoutine(Vector2 dir)
    {
        _canDash = false;
        _isDashing = true;

        if (_playerMovement != null) _playerMovement.SetMove(false);
        
        rigid.linearVelocity = dir.normalized * dashSpeed;

        //if (tr != null) tr.emitting = true;

        yield return new WaitForSeconds(dashDuration);

       // if (tr != null) tr.emitting = false;
       
        _isDashing = false;
        
        // 대시가 끝나면 다시 움직일 수 있게 켭니다.
        if (_playerMovement != null) _playerMovement.SetMove(true);

        yield return new WaitForSeconds(dashCoolTime);
        _canDash = true;
    }

    public bool IsDashing => _isDashing; 
}
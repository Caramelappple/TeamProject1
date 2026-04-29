using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isDashing = false; 

    [SerializeField] private Rigidbody2D rigid;
    private Vector2 movedir;
    private Vector2 lastdir = Vector2.down;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        rigid.linearVelocity = movedir * moveSpeed;
    }

    public void OnMove(InputValue value)
    {
        movedir = value.Get<Vector2>();
        if (movedir != Vector2.zero)
            lastdir = movedir;
    }

    public Vector2 GetLastDir() => lastdir;

    public void SetDashing(bool state) => isDashing = state;

    internal void ApplySpeedBoost(float boostMultiplier, float boostDuration)
    {
        throw new NotImplementedException();
    }
}


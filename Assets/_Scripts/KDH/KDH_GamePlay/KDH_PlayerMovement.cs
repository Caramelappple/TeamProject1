using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class KDH_PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector2 _moveDir;
    private Rigidbody2D _rigid;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        foreach (var keyPair in KeySetting.keys)
        {
            KeyAction action = keyPair.Key;
            KeyCode mappedKey = keyPair.Value; // UI에서 설정한 키보드 키

            // UI에서 설정한 해당 키를 누르면
            if (Input.GetKeyDown(mappedKey))
            {
                Debug.Log(mappedKey);
            }
        }
    }

    private void FixedUpdate()
    {
        _rigid.linearVelocity = _moveDir * _speed;
    }

    private void OnMove (InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }
}
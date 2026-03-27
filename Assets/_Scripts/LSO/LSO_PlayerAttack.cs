using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LSO_PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _swordAxis;
    private bool _Attacked = false;
    private float _speed = 20;
    private float _rotatedAngle = 0;
    private float _cooldown = 0.6f;

    private void Awake()
    {
        _swordAxis.SetActive(false);
    }
    private void FixedUpdate()
    {
        _cooldown -= Time.deltaTime;
        if (_Attacked && _cooldown <= 0)//공격 하면
        {
            _swordAxis.SetActive(true);
            _swordAxis.transform.Rotate(transform.forward * _speed);
            _rotatedAngle += _speed;//누적 회전 각도
            if (_rotatedAngle >= 360)//360도 회전하면 공격 끝
            {
                _Attacked = false;
                _rotatedAngle = 0;
                _cooldown = 0.6f;
                _swordAxis.SetActive(false);
            }
            
        }     
    }
    private void OnAttack()//공격하면 _Attacked가 true
    {
        if (!_Attacked)     
            _Attacked = true;
    }
}

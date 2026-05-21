using System;
using UnityEngine;

public class KHG_Dummyhit : MonoBehaviour
{
    private Animator _animator;
    private readonly string _enemyTag = "Enemy";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(_enemyTag))
        {   
            _animator.SetTrigger("Hit");
        }
    }
}

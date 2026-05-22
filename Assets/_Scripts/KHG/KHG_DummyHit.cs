using System;
using UnityEngine;

public class KHG_DummyHit : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Dummy"))
        {
            if (_animator != null)
            {
                _animator.SetTrigger("crow");
            }
        }
    }
}

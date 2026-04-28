using UnityEngine;

public class BearTrap : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player")) return;
        _animator.SetTrigger("Explode");
        Destroy(collision.gameObject);
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (_stateInfo.IsName("BearTrapExplode") && _stateInfo.normalizedTime >= 0.95f)//애니메이션이 끝나면 트랩 비활성화
        {
            gameObject.SetActive(false);
            BearTrapSpawner.instance.activeTraps.RemoveLast();
            BearTrapSpawner.instance.trapPool.Push(gameObject);
        }
    }
}

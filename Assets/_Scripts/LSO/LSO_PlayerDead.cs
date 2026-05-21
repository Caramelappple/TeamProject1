using UnityEngine;
using DG.Tweening;

public class LSO_PlayerDead : MonoBehaviour
{
    [SerializeField]private AudioClip clip;
    
    private LSO_PlayerAttack _attack;
    private LSO_PlayerMovement _movement;
    private Health _health;
    private SpriteRenderer _sprite;
    private Animator _animator;

    [SerializeField] private KDH_MainMenuGo _menuGo;

    private LSO_Editor _editor;
    
    private void Awake()
    {
        _health = GetComponent<Health>();
        _movement = GetComponent<LSO_PlayerMovement>();
        _attack = GetComponent<LSO_PlayerAttack>();
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _editor = LSO_Editor.Instance;
        if (!_editor) Debug.LogError("LSO_PlayerDead: editor not found");
        _health.OnDamage += (data) => Die(_health);
    }

    private void Die(Health health)
    {
        if (health.Value > health.MinValue) return;
    
        // 1. 애니메이션을 정상 실행 (컴포넌트를 바로 끄지 않음)
        LSO_SoundManager.Instance.SfxPlay("Die",clip);
        _animator.Play("DownIdle");
        DOVirtual.DelayedCall(0.02f, () => _animator.speed = 0f);
        health.Value = 0;
        health.SetDamageable(false);
        _attack.SetCanAttack(false);
        _movement.SetMove(false);
    
        SetSat(-100);
        SetTra(0);
        Up();

        _menuGo.MakeUI();
    }
    private void SetSat(float targetValue)
    {
        float startValue = _editor.colorGrading.saturation.value;

        DOTween.To(() => startValue, x => _editor.colorGrading.saturation.value = x, targetValue, 3f)
            .SetEase(Ease.OutCubic);
    }

    private void SetTra(float value)
    {
        _sprite.DOFade(value, 3f).SetEase(Ease.OutCubic);
    }

    private void Up()
    {
        transform.DOLocalMoveY(transform.position.y + 4f, 2.8f).SetEase(Ease.OutCubic);
    }
}

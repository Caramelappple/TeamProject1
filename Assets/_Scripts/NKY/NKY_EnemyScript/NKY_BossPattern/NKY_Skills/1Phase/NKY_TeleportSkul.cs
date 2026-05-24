using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

namespace _Scripts.NKY.NKY_EnemyScript.NKY_Skills
{
    public class NKY_TeleportSkul : NKY_BossSkill
    {
        [SerializeField] private Transform spawnPoints;
        [SerializeField] private GameObject skulPrefab;
        [SerializeField] private string playAnimName = "";
        [SerializeField] private GameObject bombEffectPrefab;
        [SerializeField] private Collider2D bombCollieder;
        [SerializeField] private Collider2D attackCollider;
        
        [Header("보스 스킬 스텟 세팅")]
        [SerializeField] private float spawnInterval = 0.3f;
        [SerializeField] private int swordCount = 6;
        [SerializeField] private float skulDistance = 30f;
        [SerializeField] private float skulDuration = 0.2f;
        
        [field: SerializeField] public override float DamageScale { get; protected set; }
        private Queue<GameObject> skulQueue = new Queue<GameObject>();
        private List<GameObject> skuls =  new List<GameObject>();
        private Queue<GameObject> effectQueue = new Queue<GameObject>();
        private List<GameObject> effects =  new List<GameObject>();

        //private int _damage;

        private void Start()
        {
            _damage = (int)(DamageScale * _bossBrain.Damage);
            for (int i = 0; i < swordCount; i++)
            {
                GameObject sword;
                sword = Instantiate(skulPrefab);
                sword.SetActive(false);
                skulQueue.Enqueue(sword);
            }
            for (int i = 0; i < swordCount; i++)
            {
                GameObject effect;
                effect = Instantiate(bombEffectPrefab);
                effect.SetActive(false);
                effectQueue.Enqueue(effect);
            }
        }

        public override IEnumerator Execute(Transform boss, Transform target)
        {
            float fireAngle = 0;
            skuls.Clear();
            effects.Clear();
            GameObject[] _swords = new GameObject[swordCount];
            
            for (int i = 0; i < swordCount; i++)
            {
                _swords[i] = skulQueue.Dequeue();
                NKY_CrashDamageAndTeleport crash = _swords[i].GetComponent<NKY_CrashDamageAndTeleport>();
                crash.damage = _damage;
                crash.teleportTarget = boss;
                skuls.Add(_swords[i]);
            }
            if (playAnimName != null)
            {
                Anim.Play(playAnimName);
                
                
                if ((target.position - boss.position).x < 0)
                {
                    boss.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    boss.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            foreach (GameObject skul in _swords)
            {
                Vector3 to;
                float currentAngle;
                skul.transform.position = spawnPoints.position;
                Anim.Play(playAnimName);
                fireAngle += 360 / (float)swordCount;
                skul.transform.rotation = Quaternion.Euler(0, 0, fireAngle);
                to = skul.transform.position + (skul.transform.up * skulDistance);
                currentAngle = fireAngle;
                skul.SetActive(true);
                
                NKY_SoundManager.Instance.PlaySFX("SkullThrow");//돌아갈때마다 오디오 실행
                
                StartCoroutine(PlaySequence(
                    ShowWarn(0, new Vector2(0.4f , skulDistance),
                        0.8f, () => Vector3.Lerp(skul.transform.position, to, 0.5f), currentAngle),
                    WaitUntilOrTime(() => false, 0.2f),
                    Move(skul.transform, skul.transform.up, skulDistance, skulDuration)
                ));
            }
            yield return WaitUntilOrTime(() => false, skulDuration * 1.5f);
            foreach (GameObject skul in _swords)
            {
                GameObject effect = effectQueue.Dequeue();
                StartCoroutine(PlayBomb(skul, effect, boss));
                effects.Add(effect);
            }
            Anim.Play("Attack1");
            
            NKY_SoundManager.Instance.PlaySFX("SkullAttack");//근접공격할떄 바닥 부서지는 소리
            
            yield return PlaySequence(ShowWarn(attackCollider, 0.7f, () => spawnPoints.position),
                WaitAnim("Attack1", 0.8f),
                Attack(() =>
                {
                _HitBoxController.Cast(attackCollider, spawnPoints.position, 
                    (hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage));
                })
            );
            foreach (GameObject skul in skuls)
            {
                EnQueues(skul, skulQueue);
            }
        }

        public override void EndSkill()
        {
            skuls.Clear();
            effects.Clear();
        }

        private IEnumerator PlayBomb(GameObject skul ,GameObject effect, Transform boss)
        {
            effect.transform.position = skul.transform.position;
            effect.SetActive(true);
            yield return PlaySequence(
                ShowWarn(bombCollieder, 0.4f, () => effect.transform.position),
                WaitUntilOrTime(() => false, 0.5f),
                Attack(() =>
                {
                    _HitBoxController.Cast(bombCollieder, effect.transform.position, 
                        (hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage));
                }),
                PlayBombEffect(effect)
            );
        }

        private IEnumerator PlayBombEffect(GameObject effect)
        {
            Animator effectAnim =  effect.GetComponent<Animator>();
            effect.transform.localScale = new Vector3(3f, 3f, 1f);
            effect.SetActive(true);
            
            NKY_SoundManager.Instance.PlaySFX("SkullExplode");
            
            effectAnim.Play("BombEffectAnim");
            yield return WaitAnim(effectAnim, "BombEffectAnim", 0.9f);
            EnQueues(effect, effectQueue);
        }
        
        private void EnQueues(GameObject obj, Queue<GameObject> queue)
        {
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
    }
}
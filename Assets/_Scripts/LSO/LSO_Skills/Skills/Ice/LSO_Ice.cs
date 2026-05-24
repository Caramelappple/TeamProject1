using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class LSO_Ice : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private NKY_SoundData[] soundClip;
    private int _clipIndex;
    
    private GameObject _player;
    private LSO_PlayerMovement _playerMovement;
    private bool _canUse = true;
    private Vector3 _lastDir;
    
    [SerializeField] private float _coolTime = 5f;
    private float _waitTime = 0.12f;//아이스 스파이크 사이의 시간 간격
    private int _count = 12;//소환할 개수
    private float _range = 1.8f;//플레이어를 중심으로 띄어진 거리
    private Vector3 _tempTransform;
    [SerializeField] float spreadAngle = 180f;//180이지만 실제 작동에서는 360도
    
    [SerializeField]private GameObject effect;
    private GameObject _effectInstance;//이펙트 복제한것
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
    
        _player = player;
        _playerMovement = _player.GetComponent<LSO_PlayerMovement>();
        _lastDir = _playerMovement.GetFixedLastDir();
        _tempTransform = new Vector3(player.transform.position.x, player.transform.position.y, _player.transform.position.z);
    
        StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        
        //각도 나누는거 호연이꺼 떼옴
        //각도를 구한 다음 벡터 값으로 변환해, 결과적으로 도넛모양으로 생성함
        //데미지와 삭제는 각자 아이스 스파이크에서 처리
        float baseAngle = Mathf.Atan2(_lastDir.y, _lastDir.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - (spreadAngle / 2f);
        float angleStep = spreadAngle / (_count - 1);

        for (int i = 0; i < _count-1; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            float radian = currentAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
            
            NKY_SoundManager.Instance.PlaySFX(soundClip[Random.Range(0, soundClip.Length)].soundName);
            _clipIndex = (_clipIndex + 1) % soundClip.Length;
            
            _effectInstance = Instantiate(effect, (Vector3)direction * _range + _tempTransform, Quaternion.identity);
            
            if (this != null && gameObject != null)
                _effectInstance.transform.SetParent(gameObject.transform);
            
            _effectInstance.transform.SetParent(gameObject.transform);
            yield return new WaitForSeconds(_waitTime);
        }
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}

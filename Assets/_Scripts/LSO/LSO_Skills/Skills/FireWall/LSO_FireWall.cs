using System.Collections;
using UnityEngine;

namespace _Scripts.LSO.LSO_Skills.Skills
{
    public class LSO_FireWall :  MonoBehaviour,LSO_ISkill
    {
        [SerializeField] private AudioClip clip;
        
        private GameObject _player;
        private LSO_PlayerMovement _playerMovement;
        private bool _canUse = true;
        private Vector2 _tempVector2;
        private Vector3 _lastDir;
    
        [SerializeField] private GameObject effect;
        private GameObject _effectInstance;
    
        private float _waitTime = 0.1f;
        private float _coolTime = 5f;
        private int _count = 5;
        private float _gap = 2.5f;
        public void UseSkill(GameObject player)
        {
            if (!_canUse) return;
            
            _player = player;
            _playerMovement = _player.GetComponent<LSO_PlayerMovement>();
            _lastDir = _playerMovement.GetFixedLastDir();
            _tempVector2 = new Vector2(_player.transform.position.x, _player.transform.position.y);
            
            player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
        }
        
        public IEnumerator CoolTime(float time)
        {
            _canUse = false;

            Vector2 curDir = new Vector2(_lastDir.x, _lastDir.y);

            for (int i = 1; i <= _count; i++)
            {
                LSO_SoundManager.Instance.SfxPlay(clip);
                _effectInstance = Instantiate(effect, (Vector3)curDir * (i * _gap) + (Vector3)_tempVector2, Quaternion.identity);
                _effectInstance.transform.SetParent(gameObject.transform);
                
                yield return new WaitForSeconds(_waitTime);
            }
            
            yield return new WaitForSeconds(time);
            _canUse = true;
        }
    }
}
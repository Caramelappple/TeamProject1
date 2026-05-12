using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.LSO.LSO_Skills.Skills
{
    public class LSO_FireWall :  MonoBehaviour,LSO_ISkill
    {
        private GameObject _player;
        private LSO_PlayerMovement _playerMovement;
        private bool _canUse = true;
        private Vector2 _tempVector2;
    
        [SerializeField] private GameObject effect;
        private GameObject _effectInstance;
    
        private float _waitTime = 0.1f;
        private float _coolTime = 5f;
        private int _damage = 80;
        private int _count = 5;
        private float _gap = 1;
        public void UseSkill(GameObject player)
        {
            if (!_canUse) return;
            
            _player = player;
            _playerMovement = _player.GetComponent<LSO_PlayerMovement>();
            
            _tempVector2 = new Vector2(_player.transform.position.x, _player.transform.position.y);
            
            player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
        }
        
        public IEnumerator CoolTime(float time)
        {
            _canUse = false;

            Vector2 curDir = new Vector2(_playerMovement.lastDir.x, _playerMovement.lastDir.y);

            for (int i = 1; i <= _count; i++)
            {
                _effectInstance = Instantiate(effect, (Vector3)curDir * (i * _gap) + (Vector3)_tempVector2, Quaternion.identity);
                _effectInstance.transform.SetParent(gameObject.transform);
                
                yield return new WaitForSeconds(_waitTime);
            }
            
            yield return new WaitForSeconds(time);
            _canUse = true;
        }
    }
}
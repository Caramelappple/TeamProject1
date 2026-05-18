using System.Collections;
using UnityEngine;
    public class LSO_LightningSpawner : MonoBehaviour,LSO_ISkill
    {
        private GameObject _player;
        private bool _canUse = true;
        private Collider2D[] _targets;
        private GameObject _target;
        
        private readonly float _diff = 100;
        
        private float _coolTime = 5f;
        private float _minWaitTime = 0.1f;
        private float _maxWaitTime = 0.4f;
        private int _minCount = 4;
        private int _maxCount = 10;//내리칠 횟수
        private float _range  = 1.2f;//오차 범위
        private float _nearRange = 100;
        
        [SerializeField]private GameObject effect;
        private GameObject _effectInstance;//이펙트 복제한것
        public void UseSkill(GameObject player)
        {
            if (!_canUse) return;
            
            _player = player;
            
            
            player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
        }

        public IEnumerator CoolTime(float time)
        {
            _canUse = false;
            _target = GetNearest();
            if (_target == null)
            {
                Debug.LogWarning("No target found");
                yield break;
            }
            for (int i = 0; i < Random.Range(_minCount, _maxCount); i++)
            {
                if (!_target) break;
                
                _effectInstance = Instantiate(effect, _target.transform.position+new Vector3(Random.Range(-_range,_range),Random.Range(-_range,_range)), Quaternion.identity );
                _effectInstance.transform.parent = transform;
                
                //데미지와 삭제는 각각 인스턴스에서 처리
                
                yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));
            }
            
            yield return new WaitForSeconds(time);
            _canUse = true;
        }
        
        //가장 가까운거 찾기
        private GameObject GetNearest()
        {
            _targets = Physics2D.OverlapCircleAll(_player.transform.position, _nearRange);

            GameObject result = null;
            float diff = this._diff;

            foreach (Collider2D target in _targets)
            {
                if (!target.CompareTag("Enemy")) continue;

                Vector3 myPos = transform.position;
                Vector3 targetPos = target.transform.position;
                float curDiff = Vector3.Distance(myPos, targetPos);

                if (curDiff < diff)
                {
                    diff = curDiff;
                    result = target.gameObject;
                }
            }

            return result;
        }
}
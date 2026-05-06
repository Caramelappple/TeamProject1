using UnityEngine;

namespace _Scripts.NKY._EnemyScript
{
    public class NKY_ShadowController : MonoBehaviour
    {
        [SerializeField] private Transform target; // 추적할 대상 (보스)

        private bool _isLocked; // 위치 고정 여부

        private Vector3 _position;

        void LateUpdate() // 보스의 이동이 끝난 후 실행되도록 LateUpdate 사용
        {
            if (!_isLocked && target != null)
            {
                // 보스의 X, Y 좌표를 따라가되, 오프셋을 더함
                transform.position = target.position;
            }
            else
            {
                transform.position = _position;
            }
        }

        public void MoveToLock(Vector3 position)
        {
            SetPosition(position);
            LockShadow();
        }
        // 그림자 위치 고정
        public void LockShadow()
        {
            _isLocked = true;
            _position = transform.position;
        }


        // 그림자 고정 해제 (다시 보스를 따라감)
        public void UnlockShadow() => _isLocked = false;

        // 특정 위치로 그림자 강제 이동 (필요 시)
        public void SetPosition(Vector2 pos) => transform.position = (Vector3)pos;
    }
}

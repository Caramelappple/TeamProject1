using System.Collections;
using UnityEngine;

public class LSO_PunchSpawner : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private AudioClip[] clip;
    
    [SerializeField] private GameObject effect;
    private GameObject _effectInstance;
    private GameObject _player;
    private LSO_Punch[] _punches;
    
    private LSO_PlayerMovement _movement;
    private bool _canUse = true;
    private float _coolTime = 8f;
    private float _distance = 1.5f;
    private float _waitTime = 0.07f;
    private int _count = 5;
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        _canUse = false;
        
        _player = player;
        _movement = player.GetComponent<LSO_PlayerMovement>();
        
        _movement.SetMove(false);
        _effectInstance = Instantiate(effect, player.transform.position+_movement.GetLastDir() * _distance, Quaternion.identity);
        _effectInstance.gameObject.SetActive(false);
        _punches = _effectInstance.GetComponentsInChildren<LSO_Punch>();
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        for (int i = 0; i < _count; i++)
        {
            foreach (LSO_Punch punch in _punches)
            {
                punch.Init(_player.GetComponent<Health>());
                punch.gameObject.SetActive(true);
                LSO_SoundManager.Instance.SfxPlay(clip[Random.Range(0, clip.Length)]);
                yield return new WaitForSeconds(_waitTime);
            }
        }
        _movement.SetMove(true);
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}

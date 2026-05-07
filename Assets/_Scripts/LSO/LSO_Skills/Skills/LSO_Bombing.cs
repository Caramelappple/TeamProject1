using System;
using System.Collections;
using UnityEngine;

public class LSO_Bombing : LSO_PlayerMovement,LSO_ISkill
{
    private LSO_PlayerMovement _playerMovement;
    private GameObject _player;

    private void Start()
    {
        //_player = _playerMovement.GetPlayer();
        //Debug.Log(_player);
    }

    public void UseSkill(GameObject player)
    {
        throw new NotImplementedException();
    }

    public IEnumerator CoolTime(float time)
    {
        throw new NotImplementedException();
    }
}

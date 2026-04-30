using System;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    private GameObject _skillPrefab;
    
    private ISkill _skill;

    private ISkill[]  _skillSlot = new ISkill[2];
    
    public static SkillSlot instance;
    
    private PlayerMovement _playerMovement;
    private void Awake()
    {
        instance = this;
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void AddSkill(ISkill skill,int index)
    {
        ISkill oldSkill = RemoveSkill(index);  // 기존 스킬 꺼내기
        ISkill newSkill = skill;
        if (oldSkill != null)
            _playerMovement.OnSkillEvent -= oldSkill.UseSkill;  // 기존 스킬 해제

        _skillSlot[index] = newSkill;
        _playerMovement.OnSkillEvent += newSkill.UseSkill;  // 구독
    }

    public void GetSkill(ISkill skill, int index)
    {
        _skill = skill;
    }
    
    public ISkill RemoveSkill(int index)
    {
        ISkill temp = _skillSlot[index];
        _skillSlot[index] = null;
        return temp;
    }
    
    //스킬 아이템 생성
    //스킬 아이템을 먹으면 스킬 아이템에 있는 스크립터블 오브젝트 읽기
    //슬롯 스크립트에 읽은 스크립터블 오브젝트 가져오기
    //스크립터블 오브젝트에서 프리팹을 가져온 다음 슬롯에 넣기
    //슬롯에 넣은 프리팹의 스킬 스크립트에 인터페이스 구현강제하기
    //플레이어 스크립트에 이벤트 만들기
    //이벤트에 스킬 스크립트에 있는 Use 메서드 구독 시키기
    //키 누르면 이벤트 실행시키기
     
    //목록
    //슬롯 스크립트: 스킬 교체 관리
    //SKillSo 스크립트: 스크립터블 오브젝트 생성
    //ISKill 인터페이스: 사용할 스킬에 상속 시키기
    //실제 스킬 스크립트: 이벤트 액션 Invoke
    //플레이어 무브먼트 스크립트: 이벤트 액션 선언 및 키 감지
    //실제 교체 메서드를 사용할 로직을 사용하는 클래스 하나 만들기
    
    //SkillSo 스크립터블 오브젝트 완성
    //ISkill 인터페이스 완성
}
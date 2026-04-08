using UnityEngine;

//이벤트 발생 시 전달되는 매개변수(데이터)를 담는 객체
public class KSY_DamageEventArgs
{
    public GameObject giver;
    public int damage;
    public int currentHealth;
}
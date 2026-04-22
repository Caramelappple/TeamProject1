using UnityEngine;
//Diagnostics : 진단, 검사
using System.Diagnostics;

namespace NKY.ItemSystem
{
    public interface NKY_ICollectable
    {
        //멤버(Member)는 클래스안에 있는 모든 변수와 메서드를 말합니다.
        //클래스(Class)나 객체(Object) 내부에 정의된 구성 요소(변수와 함수)
        public void Collect();


        //1. 다중 상속이 가능하고
        //2. 내부 요소들을 강제로 구현하게 합니다.
        //3.

    }















    public interface TestInterface
    {
        public int testInt { get; set; }
        public void Interface();
    }
    public abstract class TestAbstract
    {
        public int testInt;
        public void AbstractInterface()
        {
            TsetClass.TestOverloading("나는 대머리다");
        }

        public void Normal()
        {

        }
        public abstract void Abstract();
        public virtual void Virtual()
        {

        }
    }
    public class TsetClass : TestAbstract, TestInterface
    {
        public int testInt { get; set; }

        //오버로딩
        public static void TestOverloading(string text)
        {
            UnityEngine.Debug.Log(text);
        }
        public static void TestOverloading(GameObject giver, string text)
        {
            UnityEngine.Debug.Log($"보낸 사람 {giver}: {text}");
        }

        public override void Abstract()
        {
            TsetClass.TestOverloading("나는 대머리다");
            throw new System.NotImplementedException();
        }





        //인터페이스
        public void Interface()
        {
            TsetClass.TestOverloading("나는 대머리다");
            throw new System.NotImplementedException();
        }
        public void TestMethod()
        {
            TsetClass.TestOverloading("나는 대머리다");

            TsetClass tc = new TsetClass();
            TestAbstract ta = new TsetClass();
            TestInterface ti = new TsetClass();
        }
    }
}
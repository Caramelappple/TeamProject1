using UnityEngine;
//Diagnostics : 진단, 검사
using System.Diagnostics;
using System;

namespace KSY.ItemSystem
{
    public interface KSY_ICollectable
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
    public abstract class TestParent
    {
        public int testInt;
        public void AbstractInterface()
        {
            TestClass.TestOverloading("나는 대머리다");
        }

        //일반 메서드
        public void Normal_Debug(string text)
        {
            UnityEngine.Debug.Log(text);
        }
        //추상 메서드
        public abstract void Abstract_Debug();
        //가상 메서드
        public virtual void Virtual_Debug(string text)
        {
            UnityEngine.Debug.Log(text);
        }
    }
    public class TestClass : TestParent, TestInterface
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
        public override void Virtual_Debug(string text)
        {
            //base.Virtual_Debug(text);
            Console.WriteLine(text);
        }

        public override void Abstract_Debug()
        {
            TestClass.TestOverloading("나는 대머리다");
            throw new System.NotImplementedException();
        }





        //인터페이스
        public void Interface()
        {
            TestClass.TestOverloading("나는 대머리다");
            throw new System.NotImplementedException();
        }
        public void TestMethod()
        {
            TestClass.TestOverloading("나는 대머리다");

            TestClass tc = new TestClass();
            TestParent ta = new TestClass();
            TestInterface ti = new TestClass();
        }
    }
}
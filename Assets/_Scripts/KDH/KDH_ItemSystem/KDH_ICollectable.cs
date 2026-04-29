//Diagnostics : 진단, 검사
using NUnit.Framework;
using System;
using UnityEngine;

namespace KDH.ItemSystem
{
    public interface KDH_ICollectable
    {
        //멤버(Member)는 클래스안에 있는 모든 변수와 메서드를 말합니다.
        //클래스(Class)나 객체(Object) 내부에 정의된 구성 요소(변수와 함수)
        public void Collect();


        //1. 다중 상속이 가능하고
        //2. 내부 요소들을 강제로 구현하게 합니다.
    }















    public interface TestInterface
    {
        public int testInt { get; set; }
        public void Interface();
    }

    //클래스 앞에다가 abstract를 붙이는 이유 & 의미
    //1. 인스턴스화를 못하게 함 -> 개간단하게 말하면 new()를 못함.
    //2. 명시적으로 내가 abstract 멤버 (프로퍼티, 메서드)가 있음을 표현함.

    public abstract class TestParent
    {
        public int testInt;
        public void AbstractInterface()
        {
            //TestParent parent = new TestParent();
            TestParent tc = new TestClass();

            TestClass.TestOverloading("나는 대머리다");
        }

        //일반 메서드
        public void Normal_Debug(string text)
        {
            UnityEngine.Debug.Log(text);
        }
        //가상 메서드
        public virtual void Virtual_Debug(string text)
        {
            UnityEngine.Debug.Log(text);
        }
        //추상 메서드
        //abstract : 추상 << 파파고에 abstract 돌리면 어떻게 번역되는지
        //봐봐 얘들아.
        public abstract void Abstract_Debug();
    }
    //sealed : 봉인한?, 밀폐된
    public class TestClass2 : TestClass
    {
        public override void Virtual_Debug(string text)
        {
            base.Virtual_Debug(text);
        }
    }
    public sealed class TestClass3 : TestClass2
    {

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
        //public override void Virtual_Debug(string text) << 해석을 하면
        //내가 상속받고 있는 부모의 Virtual_Debug라는 이름의 메서드를
        //부모의 것을 쓰지않고 나의 것으로 쓰겠다.
        //public override void Virtual_Debug(string text)
        //{
        //    //base.Virtual_Debug(text);
        //    Console.WriteLine($"Virtual : {text}");
        //}
        public override void Abstract_Debug()
        {

        }





        //인터페이스
        public void Interface()
        {
            TestClass.TestOverloading("나는 대머리다");
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


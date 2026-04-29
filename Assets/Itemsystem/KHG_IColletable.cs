using System;
using UnityEngine;
using UnityEngine.UI;

public interface KHG_IColletable
{
   public interface KHG_Icollectable
    {
        public void Collect();

        // 인터페이스의 특징
        //1. 다중 상속이 가능
        //2. 내부 요솔들을 강제로 구현하게 할수있다
        
    }
    public interface TestInterface
    {
        public int testInt { get; set; }
        public void Interface();
    }
    public abstract class TestParent
    {
        public int testInt;
        public void Normal_Debug(string text)
        {
            Debug.Log(text);
        }

        public virtual void Virtual_Debug(string text)
        {
            Debug.Log(text);
        }

        public abstract void Abstract_Debug();
        

    }
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

        public static void TestOveroding( string text)
        {
            Debug.Log(text);
        }
        public static void TestOveroding(GameObject giver, string text)
        {
            Debug.Log($"보낸 사람 {giver}: {text}");
        }

        public override void Virtual_Debug(string text)
        {
           //base.Virtual_Debug(text);
            Console.WriteLine($"Virtual: { text}");
        }
        public override void Abstract_Debug()
        {
            
        }
       
        
        
        
        
        
        
        
        
        
        
        
        
        public void Interface()
        {
            TestClass.TestOveroding("나는 대머리다");
            throw new System.NotImplementedException();
        }
        public void TestMethod()
        {
            TestClass.TestOveroding("나는 대머리다");

            TestClass tc = new TestClass();
            TestParent ta = new TestClass();
            TestInterface ti = new TestClass();
        }
    }
}

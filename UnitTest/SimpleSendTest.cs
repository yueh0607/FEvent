using FEvent;
using System;

namespace UnitTest
{
    public interface ITestSendEvent : ISendEvent<int, string>
    {
        void This_is_a_SendMethod(int a, string b);
    }

    public class TestObject : ITestSendEvent
    {
        public void This_is_a_SendMethod(int a, string b)
        {
            Console.WriteLine($"{a},{b}");
        }

  
    }


    [TestClass]
    public class SimpleSendTest
    {
        //定向发送测试
        [TestMethod]
        public void TargetSendTest()
        {
            TestObject obj = new TestObject();
            obj.Send(1, "2");
        }

        //广播测试
        [TestMethod]
        public void GlobalSendTest()
        {
            //注册了N个
            List<TestObject> objs = new List<TestObject>();
            for (int i = 0; i < 1_0000; i++)
            {
                var obj = new TestObject();
                objs.Add(obj);
                FEvents.Publisher.Subscribe<ITestSendEvent>(obj);
            }

            FEvents.Publisher.SendAll<ITestSendEvent>(1, "2");
        }
    }
}
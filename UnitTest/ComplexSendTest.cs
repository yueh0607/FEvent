using FEvent;
using System;

namespace UnitTest
{
    public interface IComplexSendEvent : ISendEvent
    {
        void This_is_a_SendMethod();
    }

    class ComplexObject : IComplexSendEvent
    {
        long id;
        static long _id = 0;
         
        public ComplexObject()
        {
            id = _id++;
        }

        public void This_is_a_SendMethod()
        {
            //一半的几率会增加新的事件，一半几率移除当前事件
            long addNewEvent = Random.Shared.NextInt64();
            if (addNewEvent > Int64.MaxValue/2)
            {
                FEvents.Publisher.Subscribe<IComplexSendEvent>(new ComplexObject());
            }
            else
            {
                FEvents.Publisher.UnSubscribe<IComplexSendEvent>(this);
            }

            Console.WriteLine($"这是第{id}个事件");
        }
    }


    //复杂发送测试
    [TestClass]
    public class ComplexSendTest
    {

        //广播测试
        [TestMethod]
        public void GlobalSendTest()
        {
            //注册了10个对象
            List<ComplexObject> objs = new List<ComplexObject>();
            for (int i = 0; i < 10; i++)
            {
                var obj = new ComplexObject();
                objs.Add(obj);
                FEvents.Publisher.Subscribe<IComplexSendEvent>(obj);
            }

            //在执行事件时添加和移除事件
            FEvents.Publisher.SendAll<IComplexSendEvent>();

            Console.WriteLine("广播执行完毕");

            //在本次发布时应用上次结果
            FEvents.Publisher.SendAll<IComplexSendEvent>();

        }
    }
}
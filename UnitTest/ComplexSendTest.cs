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
        public long id;
        static long _id = 0;
         
        public ComplexObject()
        {
            id = _id++;
            list.Add(this);
        }

        public static List<ComplexObject> list = new List<ComplexObject>();
        public void This_is_a_SendMethod()
        {
            Console.Write($"这是第{id}个事件      ");
            //一半的几率会增加新的事件，一半几率移除当前事件
            long addNewEvent = Random.Shared.NextInt64();
            if (addNewEvent > Int64.MaxValue/2)
            {
                var obj = new ComplexObject();
                FEvents.Publisher.Subscribe<IComplexSendEvent>(obj);
                Console.WriteLine($"添加了事件:{obj.id}");
            }
            else
            {
                var rObj = list[Random.Shared.Next() % list.Count];
                FEvents.Publisher.UnSubscribe<IComplexSendEvent>(rObj);
                list.Remove(rObj);
                Console.WriteLine($"移除了事件:{rObj.id}");
            }
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
            string remain = string.Join(',', ComplexObject.list.Select(x => x.id));
            Console.WriteLine($"\n广播执行完毕  剩余事件:{remain}\n ");
            


            //在本次发布时应用上次结果
            FEvents.Publisher.SendAll<IComplexSendEvent>();

            string remain2 = string.Join(',', ComplexObject.list.Select(x => x.id));
            Console.WriteLine($"\n广播执行完毕  剩余事件:{remain2}\n ");

            //在本次发布时应用上次结果
            FEvents.Publisher.SendAll<IComplexSendEvent>();

        }

    }
}
using FEvent;
using System;

namespace UnitTest
{
    public interface ITestCallEvent : ICallEvent<int, string>
    {
        string This_is_a_CallMethod(int a);
    }

    public class TestCallObject : ITestCallEvent
    {
        public string This_is_a_CallMethod(int a)
        {
            return a.ToString();
        }


    }

    [TestClass]
    public class SimpleCallTest
    {
        [TestMethod]
        public void TargetCallTest()
        {
            // Arrange
            TestCallObject obj = new TestCallObject();
            int input = 3333;
            string expectedOutput = "3333";

            // Act
            string result = obj.Call(input);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void GlobalCallTest()
        {
            // Arrange
            List<TestCallObject> objs = new List<TestCallObject>();
            int count = 100;
            for (int i = 0; i < count; i++)
            {
                var obj = new TestCallObject();
                objs.Add(obj);
                FEvents.Publisher.Subscribe<ITestCallEvent>(obj);
            }

            // Act
            var results = FEvents.Publisher.CallAll<ITestCallEvent>(1);

            // Assert
            Assert.AreEqual(count, results.Item1);

            for(int i=0; i < count; i++)
            {
                Assert.AreEqual("1", results.Item2[i]);
            }
        }
    }
}
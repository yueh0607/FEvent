using FEvent.Sample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace FEvent.Sample
{
    class MyObject : IUpdate
    {
        public void Update(float deltaTime)
        {
            Debug.Log($"Update : {deltaTime}");
        }
    }


    public class MonoUpdate : MonoBehaviour, IEnumerable<int>
    {

        IEnumerator<int> TestGC()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return i;
            }
        }

        void Start()
        {
            Profiler
                .BeginSample("Test");
            foreach (var k in this)
            {
                int x = k;
            }

            Profiler.EndSample();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return TestGC();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

using FEvent.Sample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace FEvent.Sample
{
    class MyObject : IUpdate,IUpdate2
    {
        public void Update(float deltaTime)
        {
            Debug.Log($"Update : {deltaTime}");
        }

        public void Update2(int deltaTime0, string a)
        {
            throw new System.NotImplementedException();
        }
    }


    public class MonoUpdate : MonoBehaviour
    {


        MyObject obj = new MyObject();
        void Start()
        {
            obj.Send(0);

            FEvent.Publisher.Subscribe<IUpdate>(obj);
            FEvent.Publisher.SendAll<IUpdate>(0);
        }


    }
}

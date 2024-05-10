using FEvent.Smaple;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FEvent.Sample
{
    class MyObject : IUpdate
    {
        public void Update(float deltaTime)
        {
            Debug.Log($"Update : {deltaTime}");
        }
    }


    public class MonoUpdate : MonoBehaviour
    {
        MyObject obj = new MyObject();
        void Start()
        {
            
        }

        
    }
}

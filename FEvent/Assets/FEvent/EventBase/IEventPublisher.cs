using FEvent.Internal;
using System;
using System.Collections.Generic;

namespace FEvent
{
    public interface IEventPublisher
    {
     
        void Subscribe<T>(T obj) where T : IGenericEventBase;
        

        void UnSubscribe<T>(T obj) where T : IGenericEventBase;

        DynamicQueue<IEventListener> GetPublishableEvents<T>(Type type) where T : IGenericEventBase;
    }
}

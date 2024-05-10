using FEvent.Internal;
using System;
using System.Collections.Generic;

namespace FEvent
{
    public interface IEventPublisher
    {
        public void Subscribe<T>(T obj) where T : IGenericEventBase;
        

        public void UnSubscribe<T>(T obj) where T : IGenericEventBase;

        public DynamicQueue<IEventListener> GetPublishableEvents<T>(Type type) where T : IGenericEventBase;
    }
}

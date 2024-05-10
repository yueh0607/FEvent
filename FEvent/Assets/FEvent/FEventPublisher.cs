using FEvent.Internal;
using System;
using System.Collections.Generic;

namespace FEvent
{
    public class FEventPublisher : IEventPublisher
    {
        internal Dictionary<Type, List<object>> m_EventContainer = new Dictionary<Type, List<object>>();

        internal void InternalAddEvent(Type type, object obj)
        {
            if (!m_EventContainer.ContainsKey(type))
            {
                m_EventContainer[type] = new List<object>();
            }
            m_EventContainer[type].Add(obj);
        }
        public void AddEvent<T>(T obj) where T : IGenericEventBase
            => InternalAddEvent(typeof(T), obj);

        internal void InternalRemoveEvent(Type type, object obj)
        {
            if (m_EventContainer.ContainsKey(type))
            {
                m_EventContainer[type].Remove(obj);
            }
        }

        public void RemoveEvent<T>(T obj) where T : IGenericEventBase
            => InternalRemoveEvent(typeof(T), obj);

        internal IReadOnlyList<object> InternalGetPublishableEvents(Type type)
        {
            if (m_EventContainer.ContainsKey(type))
            {
                return m_EventContainer[type];
            }
            return null;
        }

        public IReadOnlyList<object> GetPublishableEvents<T>(Type type) where T : IGenericEventBase
            => InternalGetPublishableEvents(type);


    }
}

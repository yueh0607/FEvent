using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using FEvent.Internal;
namespace FEvent
{
    public static class FEvent
    {
        internal static Dictionary<Type, List<object>> m_EventContainer = new Dictionary<Type, List<object>>();

        internal static void InternalAddEvent(Type type,object obj) 
        {
            if (!m_EventContainer.ContainsKey(type))
            {
                m_EventContainer[type] = new List<object>();
            }
            m_EventContainer[type].Add(obj);
        }
        internal static void AddEvent<T>(T obj) where T : IGenericEventBase
            => InternalAddEvent(typeof(T), obj);


        internal static void InternalRemoveEvent(Type type,object obj)
        {
            if (m_EventContainer.ContainsKey(type))
            {
                m_EventContainer[type].Remove(obj);
            }
        }

        internal static void RemoveEvent<T>(T obj) where T : IGenericEventBase
            => InternalRemoveEvent(typeof(T), obj);

        internal static List<object> InternalGetPublishableEvents(Type type)
        {
            if(m_EventContainer.ContainsKey(type))
            {
                return m_EventContainer[type];
            }
            return null;
        }

        internal static List<object> GetPublishableEvents<T>(Type type) where T : IGenericEventBase
            => InternalGetPublishableEvents(type);
    }
}

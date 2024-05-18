using System;
using System.Collections;
using System.Collections.Generic;

namespace FEvent
{
    public class DynamicQueue<T>
    {
        private Queue<T> m_InQueue = new Queue<T>();

        private Queue<T> m_WaitQueue = new Queue<T>();

        private HashSet<T> m_Exist = new HashSet<T>();


        private bool m_IsEnumerating = false;
        private int m_EnumCount = 0;

        public int Count => m_Exist.Count;

        public void Add(T obj)
        {
            if (m_IsEnumerating)
            {
                m_WaitQueue.Enqueue(obj);
            }
            else if (!m_Exist.Contains(obj))
            {
                m_InQueue.Enqueue(obj);
                m_Exist.Add(obj);
            }
        }

        public void Remove(T obj)
        {
            m_Exist.Remove(obj);
        }

        public void StartEnum()
        {
            m_EnumCount = m_Exist.Count;
            m_IsEnumerating = true;
        }


        public bool MoveNext(out T value)
        {
            if (m_EnumCount-- > 0)
            {
                value = m_InQueue.Dequeue();
                while (!m_Exist.Contains(value))
                {
                    value = m_InQueue.Dequeue();
                }
                return true;
            }
            value = default;
            return false;
        }

        public void Return(T value)
        {
            m_InQueue.Enqueue(value);
        }

        public void EndEnum()
        {
            while (m_WaitQueue.Count > 0)
            {
                var wait_Value = m_WaitQueue.Dequeue();
                if (!m_Exist.Contains(wait_Value))
                {
                    m_InQueue.Enqueue(wait_Value);
                    m_Exist.Add(wait_Value);
                }
            }
            m_IsEnumerating = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace FEvent
{
    public class DynamicQueue<T>
    {
        private enum DynamicCommand
        {
            Add,
            Remove,
        }

        private Queue<T> m_InQueue = new Queue<T>();

        private Queue<ValueTuple<DynamicCommand, T>> m_WaitQueue = new Queue<ValueTuple<DynamicCommand, T>>();

        private HashSet<T> m_Exist = new HashSet<T>();


        private bool m_IsEnumerating = false;
        private int m_EnumCount = 0;


        public void Add(T obj)
        {
            if (m_IsEnumerating)
            {
                m_WaitQueue.Enqueue((DynamicCommand.Add, obj));
            }
            else
            {
                m_InQueue.Enqueue(obj);
                m_Exist.Add(obj);
            }
        }

        public void Remove(T obj)
        {
            if (m_IsEnumerating)
            {
                m_WaitQueue.Enqueue((DynamicCommand.Remove, obj));
            }
            else
            {
                m_Exist.Add(obj);
            }
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
                var cmd = m_WaitQueue.Dequeue();
                if (cmd.Item1 == DynamicCommand.Add)
                {
                    T value = cmd.Item2;
                    m_InQueue.Enqueue(value);
                    m_Exist.Add(value);
                }
                else
                {
                    T value = cmd.Item2;
                    if (m_Exist.Contains(value))
                    {
                        m_Exist.Remove(value);
                    }
                }
            }
            m_IsEnumerating = false;
        }
    }
}

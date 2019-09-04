using System;
using System.Collections.Generic;

namespace Aci.UI.Collections
{
    public class Pool<T>
    {
        private const int s_DefaultSize = 10;
        private Func<T> m_Constructor;
        private int m_InitialSize;
        private Stack<T> m_Pool;

        public Pool(Func<T> constructor)
        {
            m_Constructor = constructor;
            m_InitialSize = s_DefaultSize;
            Initialize();
        }

        public Pool(Func<T> constructor, int initialSize)
        {
            m_Constructor = constructor;
            m_InitialSize = initialSize;
            Initialize();
        }

        private void Initialize()
        {
            m_Pool = new Stack<T>(m_InitialSize * 2); 
            for(int i = 0; i < m_InitialSize; i++)
            {
                m_Pool.Push(m_Constructor.Invoke());
            }
        }

        public void Clear()
        {
            m_Pool.Clear();
        }

        public T Get()
        {
            if(m_Pool.Count > 0)
            {
                return m_Pool.Pop();
            }
            else
            {
                for(int i = 0; i < m_InitialSize; i++)
                {
                    m_Pool.Push(m_Constructor.Invoke());
                }

                return m_Pool.Pop();
            }
        }

        public void Return(T item)
        {
            m_Pool.Push(item);
        }
    }
}

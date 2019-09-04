using System;
using System.Collections;
using System.Collections.Generic;

namespace Aci.Collections
{
    public abstract class Heap<TItem> : ICollection<TItem>, IReadOnlyCollection<TItem> 
    {
        private const int s_DefaultCapacity = 10;
        private static TItem[] s_Empty;
        
        protected TItem[] m_Items;

        public int Count => m_Size;

        public bool IsReadOnly => true;

        protected int m_Size;
        private int m_Capacity;


        private int Capacity
        {
            get { return m_Items.Length; }
            set
            {
                if(value != m_Items.Length)
                {
                    if(value > 0)
                    {
                        TItem[] newItems = new TItem[value];
                        if (m_Size > 0)
                            Array.Copy(m_Items, 0, newItems, 0, m_Size);

                        m_Items = newItems;
                    }
                }
                else
                {
                    m_Items = s_Empty;
                }
            }
        }

        public Heap()
        {
            m_Size = 0;
            m_Items = new TItem[s_DefaultCapacity];
        }

        public Heap(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be larger or equal to 0!");

            m_Size = 0;
            m_Items = new TItem[capacity];
        }

        void ICollection<TItem>.Add(TItem item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            if(m_Size > 0)
            {
                Array.Clear(m_Items, 0, m_Size);
                m_Size = 0;
            }
        }

        public bool Contains(TItem item)
        {
            if (m_Size == 0)
                return false;

            EqualityComparer<TItem> c = EqualityComparer<TItem>.Default;

            for (int i = 0; i < m_Items.Length; i++)
            {
                if (c.Equals(m_Items[i], item))
                    return true;
            }

            return false;
        }        

        void ICollection<TItem>.CopyTo(TItem[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            for (int i = 0; i < m_Items.Length; i++)
                yield return m_Items[i];
        }

        public bool Remove(TItem item)
        {
            int index = Array.IndexOf(m_Items, item);
            if (index == -1)
                return false;

            return Remove(index);
        }

        protected bool Remove(int index)
        {
            if (index < 0 || index >= m_Size)
                throw new IndexOutOfRangeException();

            Swap(index, m_Size - 1);
            m_Size--;
            HeapifyDown(index);

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        protected void EnsureCapacity(int min)
        {
            if(m_Items.Length < min)
            {
                int newCapacity = m_Items.Length == 0 ? s_DefaultCapacity : m_Items.Length * 2;
                if (newCapacity < min)
                    newCapacity = min;

                Capacity = newCapacity;
            }
        }

        protected int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        protected int GetLeftChildIndex(int index)
        {
            return 2 * index + 1;
        }

        protected int GetRightChildIndex(int index)
        {
            return 2 * index + 2;
        }

        protected abstract void Build();

        public void Push(TItem item)
        {
            if (m_Size == m_Items.Length)
                EnsureCapacity(m_Size + 1);

            m_Items[m_Size++] = item;
            HeapifyUp(m_Size - 1);
        }

        public TItem Peek()
        {
            if (m_Size == 0)
                throw new IndexOutOfRangeException();

            return m_Items[0];
        }

        public TItem Pop()
        {
            if (m_Size == 0)
                throw new IndexOutOfRangeException();

            TItem item = Peek();
            Remove(0);
            return item;
        }

        protected abstract void HeapifyUp(int index);

        protected abstract void HeapifyDown(int index);

        protected void Swap(int a, int b)
        {
            TItem temp = m_Items[a];
            m_Items[a] = m_Items[b];
            m_Items[b] = temp;
        }
    }
}
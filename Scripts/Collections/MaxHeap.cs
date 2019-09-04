using System;

namespace Aci.Collections
{
    public class MaxHeap<TItem> : Heap<TItem> where TItem : IComparable<TItem>
    {
        public MaxHeap() { }

        public MaxHeap(int capacity) : base(capacity) { }

        protected override void Build()
        {
            for (int i = m_Size / 2 - 1; i >= 0; i--)
                HeapifyDown(i);
        }

        protected override void HeapifyDown(int index)
        {
            int largest = index;
            int leftIndex = GetLeftChildIndex(index);
            int rightIndex = GetRightChildIndex(index);

            if (leftIndex < m_Size && m_Items[leftIndex].CompareTo(m_Items[largest]) > 0)
                largest = leftIndex;

            if (rightIndex < m_Size && m_Items[rightIndex].CompareTo(m_Items[largest]) > 0)
                largest = rightIndex;

            if(largest != index)
            {
                Swap(index, largest);

                HeapifyDown(largest);
            }
        }

        protected override void HeapifyUp(int index)
        {
            if (index == 0)
                return;

            int parentIndex = GetParentIndex(index);
            if(m_Items[index].CompareTo(m_Items[parentIndex]) > 0)
            {
                Swap(parentIndex, index);
                HeapifyUp(parentIndex);
            }
        }
    }
}

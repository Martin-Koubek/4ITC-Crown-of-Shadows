using System;
using System.Collections.Generic;

// Simple binary-heap priority queue (min-heap).
// T musí implementovat IComparable<T> (tvùj PathNode to má).
public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> data = new List<T>();

    public int Count => data.Count;

    public void Enqueue(T item)
    {
        data.Add(item);
        int ci = data.Count - 1;
        while (ci > 0)
        {
            int pi = (ci - 1) / 2;
            if (data[ci].CompareTo(data[pi]) >= 0) break;
            // swap
            T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
            ci = pi;
        }
    }

    public T Dequeue()
    {
        if (data.Count == 0) throw new InvalidOperationException("Queue is empty");
        int li = data.Count - 1;
        T frontItem = data[0];
        data[0] = data[li];
        data.RemoveAt(li);
        --li;
        int pi = 0;
        while (true)
        {
            int l = pi * 2 + 1;
            if (l > li) break;
            int r = l + 1;
            int smallest = l;
            if (r <= li && data[r].CompareTo(data[l]) < 0) smallest = r;
            if (data[smallest].CompareTo(data[pi]) >= 0) break;
            // swap
            T tmp = data[pi]; data[pi] = data[smallest]; data[smallest] = tmp;
            pi = smallest;
        }
        return frontItem;
    }

    public T Peek()
    {
        if (data.Count == 0) throw new InvalidOperationException("Queue is empty");
        return data[0];
    }

    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    public void Clear() => data.Clear();
}
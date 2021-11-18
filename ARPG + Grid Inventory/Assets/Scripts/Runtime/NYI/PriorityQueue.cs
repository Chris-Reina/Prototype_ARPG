﻿using System;
using System.Collections.Generic;

namespace DoaT.Pathfinding
{
    public class PriorityQueue <T> where T : IComparable <T>
    {
        private List <T> data;
        public int Count => data.Count;

        public PriorityQueue()
        {
            data = new List <T>();
        }
        
        public void Enqueue(T item)
        {
            data.Add(item);
            
            int childIndex = data.Count - 1;
            while (childIndex  > 0)
            {
                int parentIndex = (childIndex - 1) / 2;
                if (data[childIndex].CompareTo(data[parentIndex]) >= 0)
                    break;
                
                T tmp = data[childIndex]; 
                data[childIndex] = data[parentIndex];
                data[parentIndex] = tmp;
                childIndex = parentIndex;
            }
        }
        
        public T Dequeue()
        {
            if (data.Count == 0)
                throw new ArgumentOutOfRangeException();
            
            // Assumes Queue isn't empty
            int lastIndex = data.Count - 1;
            T frontItem = data[0];
            data[0] = data[lastIndex];
            data.RemoveAt(lastIndex);

            --lastIndex;
            int parentIndex = 0;
            while (true)
            {
                int childIndex = parentIndex * 2 + 1;
                if (childIndex  > lastIndex) break;
                
                int rc = childIndex + 1;
                if (rc  <= lastIndex && data[rc].CompareTo(data[childIndex])  < 0)
                    childIndex = rc;
                
                if (data[parentIndex].CompareTo(data[childIndex])  <= 0) break;
                
                T tmp = data[parentIndex]; 
                data[parentIndex] = data[childIndex]; 
                data[childIndex] = tmp;
                parentIndex = childIndex;
            }
            return frontItem;
        }

        public void Add(T item)
        {
            Enqueue(item);
        }

        public T Remove()
        {
            return Dequeue();
        }

        public bool Contains(T item)
        {
            return data.Contains(item);
        }
        
        public T Peek()
        {
            T frontItem = data[0];
            return frontItem;
        }
        
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i  < data.Count; ++i)
                s += data[i].ToString() + " ";
            s += "count = " + data.Count;
            return s;
        }
    } 
}
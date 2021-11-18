using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DoaT.AI
{
    [Serializable]
    public class Path
    {
        public List<Vector3> nodes;

        public Vector3 initialPosition;
    
        public int Count => nodes.Count;
    
        //[IndexerName("TheItem")]
        public Vector3 this[int index]
        {
            get => nodes?[index] ?? default;
            set
            {
                if (nodes == null) return;
                if (index <= 0 || index >= nodes.Count) return;
                
                nodes[index] = value;
            }
        }

        public Path()
        {
            nodes = new List<Vector3>();
            initialPosition = new Vector3(0, 0, 0);
        }
        
        public Path(List<Vector3> nodesList)
        {
            nodes = nodesList;
        }

        public Path(Path oldPath)
        {
            nodes = new List<Vector3>(oldPath.nodes);
        }
        
        public Path(List<Vector3> nodesList, Vector3 finalPoint)
        {
            nodes = nodesList;
            //nodes.Add(finalPoint);
        }

        public float Distance(Vector3 startPosition)
        {
            switch (Count)
            {
                case 0:
                    return 0;
                case 1:
                    return Vector3.Distance(startPosition, nodes[0]);
                default:
                    var dist = Vector3.Distance(startPosition, nodes[0]);
            
                    for (int i = 0; i < nodes.Count-1; i++)
                    {
                        dist += Vector3.Distance(nodes[i], nodes[i + 1]);
                    }

                    return dist;
            }
        }
        
        public float Distance(Vector3 startPosition, int index)
        {
            switch (Count)
            {
                case 0:
                    return 0;
                case 1:
                    return Vector3.Distance(startPosition, nodes[0]);
                default:
                    var dist = Vector3.Distance(startPosition, nodes[index]);
            
                    for (int i = index; i < nodes.Count-1; i++)//WIP
                    {
                        dist += Vector3.Distance(nodes[i], nodes[i + 1]);
                    }

                    return dist;
            }
        }

        public Vector3 NextNodeDirection(int index)
        {
            index = index >= 0 ? index : -index;
            
            if( Count-1 < 0 || index >= Count-1 || Count == 0 || Count == 1) return Vector3.zero;

            return (nodes[index + 1] - nodes[index]).normalized;
        }
    }
}


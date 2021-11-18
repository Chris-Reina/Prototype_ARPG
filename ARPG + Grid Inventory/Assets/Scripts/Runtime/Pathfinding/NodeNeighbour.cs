using System;

namespace DoaT.AI
{
    [Serializable]
    public class NodeNeighbour
    {
        public Node node;
        public float distance;

        public NodeNeighbour(Node neigh, float dist)
        {
            node = neigh;
            distance = dist;
        }
    }
}
    

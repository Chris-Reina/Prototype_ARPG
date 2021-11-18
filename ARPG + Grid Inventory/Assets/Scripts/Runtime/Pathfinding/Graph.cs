using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DoaT.AI
{
    public class Graph : MonoBehaviour
    {
        private const float NodeCheckRadius = 5f;
        
        [SerializeField] private List<Node> nodeList;

        public List<Node> NodeList => nodeList;

        private void Awake()
        {
            RecalculateNodeList();
        }
        
        public Node NodeFromWorldPosition(Vector3 worldPosition)
        {
            Node bestNode = default;

            for (int i = 0; i < 1000; i++)
            {
                var nodes = Physics.OverlapSphere(worldPosition, NodeCheckRadius + NodeCheckRadius * i,
                    LayersUtility.NodeMask, QueryTriggerInteraction.Collide);
                
                if(nodes.Length == 0) continue;
            
                bestNode = nodes
                    .OrderBy(n => Vector3.Distance(worldPosition, n.transform.position))
                    .First()
                    .GetComponent<Node>();

                break;
            }

            return bestNode;
        }

        public void ResetNodes()
        {
            foreach (var node in nodeList)
            {
                node.Reset();
            }
        }
        
        public void CalculateNodeNeighbours()
        {
            foreach (var node in nodeList)
            {
                node.CalculateNeighbours();
            }
        }
        
        public void CalculateNodeBlockState()
        {
            foreach (var node in nodeList)
            {
                node.SetBlockState(LayersUtility.WallMask);
            }

            var newList = new List<Node>(nodeList);
            
            foreach (var node in nodeList)
            {
                if (node.isBlocked)
                {
                    newList.Remove(node);
                    
                    if(Application.isEditor)
                        DestroyImmediate(node.gameObject);
                    else
                        Destroy(node.gameObject);
                }
            }

            nodeList = new List<Node>(newList);
        }

        public void RecalculateNodeList()
        {
            nodeList = new List<Node>();

            var temp = GetComponentsInChildren<Node>();

            foreach (var node in temp)
            {
                nodeList.Add(node);
            }
        }

        public Graph SetNodeList(IEnumerable<Node> list)
        {
            nodeList = new List<Node>(list);
            return this;
        }
    }
}


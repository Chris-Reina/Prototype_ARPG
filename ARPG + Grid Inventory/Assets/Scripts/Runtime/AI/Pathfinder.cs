using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoaT.AI
{
    public class Pathfinder : MonoBehaviour
    {
        [SerializeField] private Graph m_nodeGraph;

        [Range(1f, 100f)] public int stepsMaxDefault = 100;
        
        public Graph Graph => m_nodeGraph;
        
        private void Awake()
        {
            m_nodeGraph = FindObjectOfType<Graph>();
        }

        public Path GetPathStruct(Vector3 startPosition, Vector3 targetPosition, int steps = -1)
        {
            /*return steps == -1
                ? new Path(GetPathPriorityQueue(startPosition, targetPosition, stepsMaxDefault), targetPosition)
                : new Path(GetPathPriorityQueue(startPosition, targetPosition, Math.Abs(steps)), targetPosition);*/

            return new Path(GetPathPriorityQueue(startPosition, targetPosition, stepsMaxDefault), targetPosition);
        }

        #region Paths

        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        /*private List<Vector3> GetApproximatePath(Vector3 startPosition, Vector3 targetPosition)
        {
            Graph.ResetNodes();
            
            var startNode = m_nodeGraph.NodeFromWorldPosition(startPosition);
            var targetNode = m_nodeGraph.NodeFromWorldPosition(targetPosition);
            
            startNode.gCost = 0;
            startNode.hCost = MathUtility.ManhattanDistance(targetNode.Position, startNode.Position);


            var lowQ = LowQualityPath(startNode, targetNode, 20).Select(n => n.Position).ToList();
            return lowQ;
        }*/
        /*private List<Node> LowQualityPath(Node start, Node finish, int maxSteps, int currentSteps = 1)
        {
            if (maxSteps == currentSteps)
            {
                return new List<Node>();
            }

            foreach (var node in start.neighbours)
            {
                if(node.gCost != float.MaxValue) continue;

                node.hCost = MathUtility.ManhattanDistance(node.Position, finish.Position);
                node.gCost = start.gCost + Vector3.Distance(start.transform.position, node.transform.position);
            }
            
            var best = start.neighbours.OrderBy(n => n.hCost).First();
            if (best == default || best.hCost > start.hCost)
            {
                return new List<Node>();
            }

            if (best == finish)
            {
                return new List<Node>() {finish};
            }

            
            var list = LowQualityPath(best, finish, maxSteps, currentSteps + 1);
            list.Insert(0, best);
            return list;
        }*/
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        
        /*private List<Vector3> GetPath(Vector3 startPosition, Vector3 targetPosition, int stepsMax)
        {
            //var firstTime = Time.realtimeSinceStartup;
            //Debug.Log($"I'm calculating a path!!! Start position: {startPosition} --- Target Position {targetPosition}");
            Graph.ResetNodes();

            var startNode = m_nodeGraph.NodeFromWorldPosition(startPosition);
            var targetNode = m_nodeGraph.NodeFromWorldPosition(targetPosition);

            //-----First Visibility Pass
            // var dist = Vector3.Distance(startNode.Position, targetNode.Position);
            // var dir = (targetNode.Position - startNode.Position).normalized;
            // var ray = new Ray(startNode.Position, dir);
            //
            // if (!Physics.SphereCast(ray, startNode.Radius, dist, LayersUtility.WallMask))
            // {
            //     targetNode.previousNode = startNode;
            //     var firstPassPath = !smoothPath ? RetracePath(startNode, targetNode) : RetracePathSmooth(startNode, targetNode);
            //
            //     Debug.Log(((Time.realtimeSinceStartup - firstTime) * 1000) + "ms   elapsed when path was done. In first Pass.");
            //     return firstPassPath;
            // }
            
            //--- Second Pathfinding Pass
            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();
            
            openSet.Add(startNode);
            startNode.gCost = 0;
            var steps = stepsMax;
        
            var current = startNode;
            current.pathNumber = 1;
            
            while (openSet.Count > 0)
            {
                current = openSet.OrderBy(n => n.FCost).First();
        
                openSet.Remove(current);
                closedSet.Add(current);
        
                if (current == targetNode || current.pathNumber >= stepsMax)
                {
                    break;
                }

                var target = targetNode.Position;
                foreach (var neighbour in current.neighbours.Where(neighbour => !closedSet.Contains(neighbour)))
                {
                    neighbour.pathNumber = current.pathNumber + 1;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    
                        var positionTar = targetNode.transform.position;
                        var positionNeigh = neighbour.transform.position;
                        neighbour.hCost = Mathf.Abs(positionTar.x - positionNeigh.x)
                                          + Mathf.Abs(positionTar.y - positionNeigh.y)
                                          + Mathf.Abs(positionTar.z - positionNeigh.z);
                    }
                    
                    var distanceToNeighbour = Vector3.Distance(current.transform.position, neighbour.transform.position);
                    var newG = current.gCost + distanceToNeighbour;
                    
                    if ((newG >= neighbour.gCost)) continue;
                    
                    neighbour.gCost = newG;
                    neighbour.previousNode = current;
                }
            }

            if (current.pathNumber >= stepsMax)
            {
                targetNode = current;
            }
            
            //Debug.Log(((Time.realtimeSinceStartup - firstTime) * 1000) + "ms   elapsed when path ready for Smoothing.");
            //var smoothingTime = Time.realtimeSinceStartup;
            var path = RetracePathSmooth(startNode, targetNode);
        
            //Debug.Log("I'm RETURNING a path!!! with " + path.Count + " nodes.");
        
            //Debug.Log(((Time.realtimeSinceStartup - smoothingTime) * 1000) + "ms  were spent smoothing.");
            //var endTime = Time.realtimeSinceStartup;
            //Debug.Log(((endTime - firstTime) * 1000) + "ms   elapsed when path was done. List.");
            return path;
        }*/

        #endregion

        private List<Vector3> GetPathPriorityQueue(Vector3 startPosition, Vector3 targetPosition, int stepsMax)
        {
            //var firstTime = Time.realtimeSinceStartup;
            //Debug.Log($"I'm calculating a path!!! Start position: {startPosition} --- Target Position {targetPosition}");
            Graph.ResetNodes();

            var startNode = m_nodeGraph.NodeFromWorldPosition(startPosition);
            var targetNode = m_nodeGraph.NodeFromWorldPosition(targetPosition);

            //-----First Visibility Pass
            var dist = Vector3.Distance(startNode.Position, targetNode.Position);
            var dir = (targetNode.Position - startNode.Position).normalized;
            var ray = new Ray(startNode.Position, dir);
            
            if (!Physics.SphereCast(ray, startNode.Radius, dist, LayersUtility.WallMask))
            {
                targetNode.previousNode = startNode;
                var firstPassPath = RetracePathSmooth(startNode, targetNode);
            
                //Debug.Log(((Time.realtimeSinceStartup - firstTime) * 1000) + "ms   elapsed when path was done. In first Pass.");
                return firstPassPath;
            }
            
            //--- Second Pathfinding Pass
            //var openSet = new List<Node>();
            
            var openSet = new PriorityQueue<Node>();
            var closedSet = new HashSet<Node>();
            
            openSet.Enqueue(startNode);
            startNode.gCost = 0;
            startNode.hCost = MathUtility.ManhattanDistance(startNode.Position, targetNode.Position);

            var current = startNode;
            current.pathNumber = 1;
            
            while (openSet.Count > 0)
            {
                current = openSet.Dequeue();
                closedSet.Add(current);
        
                if (current == targetNode || current.pathNumber >= stepsMax)
                {
                    break;
                }

                foreach (var neighbour in current.neighbours.Where(neighbour => !closedSet.Contains(neighbour.node)))
                {
                    neighbour.node.pathNumber = current.pathNumber + 1;

                    if (!openSet.Contains(neighbour.node))
                    {
                        openSet.Enqueue(neighbour.node);
                    
                        var positionTar = targetNode.Position;
                        var positionNeigh = neighbour.node.Position;
                        neighbour.node.hCost = Mathf.Abs(positionTar.x - positionNeigh.x)
                                             + Mathf.Abs(positionTar.y - positionNeigh.y)
                                             + Mathf.Abs(positionTar.z - positionNeigh.z);
                    }
                    
                    
                    var newG = current.gCost + neighbour.distance;
                    
                    if ((newG >= neighbour.node.gCost)) continue;
                    
                    neighbour.node.gCost = newG;
                    neighbour.node.previousNode = current;
                }
                
                // foreach (var neighbour in current.neighbours.Where(neighbour => !closedSet.Contains(neighbour.node))) //IA2-P3 Where
                // {
                //     //if(neighbour.node.pathNumber != -1) continue;
                //     neighbour.node.pathNumber = current.pathNumber + 1;
                //
                //     var positionTar = targetNode.transform.position;
                //     var positionNeigh = neighbour.node.transform.position;
                //     neighbour.node.hCost = Mathf.Abs(positionTar.x - positionNeigh.x)
                //                             + Mathf.Abs(positionTar.y - positionNeigh.y)
                //                             + Mathf.Abs(positionTar.z - positionNeigh.z);
                //
                //     var newG = current.gCost + neighbour.distance;
                //     
                //     if ((newG >= neighbour.node.gCost)) continue;
                //     
                //     neighbour.node.gCost = current.gCost + neighbour.distance;
                //     neighbour.node.previousNode = current;
                //
                //     openSet.Enqueue(neighbour.node, neighbour.node.FCost);
                // }
            }
            
            
            if (current.pathNumber >= stepsMax)
            {
                targetNode = current;
                Debug.LogWarning(current.pathNumber >= stepsMax);
            }
            
            //Debug.Log(((Time.realtimeSinceStartup - firstTime) * 1000) + "ms   elapsed when path ready for Smoothing.");
            //var smoothingTime = Time.realtimeSinceStartup;
            //var path = RetracePath(startNode, targetNode);
            var path = RetracePathSmooth(startNode, targetNode);
        
            //Debug.Log("I'm RETURNING a path!!! with " + path.Count + " nodes.");
        
            //Debug.Log(((Time.realtimeSinceStartup - smoothingTime) * 1000) + "ms  were spent smoothing.");
            //var endTime = Time.realtimeSinceStartup;
            //Debug.Log(((endTime - firstTime) * 1000) + "ms   elapsed when path was done. List.");
            return path;
        }

        private List<Node> SmoothPath(List<Node> originalPath)
        {
            var currentNode = originalPath[0];
        
            var watchdog = 1000;
        
            while (currentNode)
            {
                if (watchdog-- == 0)
                {
                    return originalPath;
                    //throw new System.Exception("oh shiiiiiiiiii...");
                }
        
                if (currentNode.previousNode != null && currentNode.previousNode.previousNode != null)
                {                
                    var temp = GetLastVisibleNode(currentNode, currentNode.previousNode);
        
                    // if (temp == default(Node))
                    //     throw new System.Exception("WHYYYYYY");
        
                    //Debug.Log($"last visible parent of {currentNode.gameObject.name} is {temp.gameObject.name}");
        
                    currentNode.previousNode = temp;
                    currentNode = currentNode.previousNode;
                }
                else
                {
                    currentNode = currentNode.previousNode;
                }
            }
        
            var pathNode = RetraceNodePath(originalPath[originalPath.Count - 1], originalPath[0]);
        
            //Debug.Log($"Path Count: {pathNode.Count}");
        
            return pathNode;
        }
        private Node GetLastVisibleNode(Node initialNode, Node child)
        {
            if (child == null || initialNode == child)
                return initialNode.previousNode;
        
            if (child.previousNode == null)
                return child;

            if (child == initialNode.previousNode)
            {
                var obj = GetLastVisibleNode(initialNode, child.previousNode);
        
                return obj == null ? child : obj;
            }
        
            var dist = Vector3.Distance(initialNode.transform.position, child.transform.position);
            var dir = (child.transform.position - initialNode.transform.position).normalized;
            var ray = new Ray(initialNode.transform.position, dir);
        
            if (Physics.SphereCast(ray, initialNode.Radius, dist, LayersUtility.WallMask) 
            || !MathUtility.FastApproximately(child.Position.y, initialNode.Position.y, 0.0001f))
            {
                return null;
            }
        
            var temp = GetLastVisibleNode(initialNode, child.previousNode);
        
            return temp == null ? child : temp;
        }
        private List<Node> RetraceNodePath(Node start, Node end)
        {
            var pathNode = new List<Node>();
            var currentNode = end;
        
            while (currentNode != start)
            {
                pathNode.Add(currentNode);
                currentNode = currentNode.previousNode;
            }
            pathNode.Add(currentNode);
        
            return pathNode;
        }

        private List<Vector3> RetracePathSmooth(Node start, Node end)
        {
            var pathNode = new List<Node>();
            var pathPositionNode = new List<Vector3>();
            
            var currentNode = end;
        
            while (currentNode != start && currentNode.previousNode)
            {
                pathNode.Add(currentNode);
                currentNode = currentNode.previousNode;
            }
            pathNode.Add(currentNode);
        
        
            var originalPath = new List<Node>(pathNode);
            pathNode = SmoothPath(pathNode);
        
            for (int i = 0; i < pathNode.Count; i++)
            {
                if (!pathNode[i])
                    pathNode.RemoveAt(i);
        
                if(!originalPath[i])
                    originalPath.RemoveAt(i);
            }
        
            while (pathNode.Count > 0)
            {
                pathPositionNode.Add(pathNode[pathNode.Count - 1].transform.position);
                pathNode.RemoveAt(pathNode.Count - 1);
            }
        
            while (originalPath.Count > 0)
            {
                //pathOriginalPositionNode.Add(originalPath[originalPath.Count - 1].transform.position);
                originalPath.RemoveAt(originalPath.Count - 1);
            }
        
            //original = pathOriginalPositionNode;
        
            return pathPositionNode;
        }

        private List<Vector3> RetracePath(Node start, Node end)
        {
            var pathNode = new List<Node>();
            var pathPositionNode = new List<Vector3>();
            
            var currentNode = end;
        
            while (currentNode != start && currentNode.previousNode)
            {
                pathNode.Add(currentNode);
                currentNode = currentNode.previousNode;
            }
            pathNode.Add(currentNode);

            var originalPath = new List<Node>(pathNode);
        
            /*for (int i = 0; i < pathNode.Count; i++)
            {
                if (!pathNode[i])
                    pathNode.RemoveAt(i);
        
                if(!originalPath[i])
                    originalPath.RemoveAt(i);
            }
        
            while (pathNode.Count > 0)
            {
                pathPositionNode.Add(pathNode[pathNode.Count - 1].transform.position);
                pathNode.RemoveAt(pathNode.Count - 1);
            }
        
            while (originalPath.Count > 0)
            {
                //pathOriginalPositionNode.Add(originalPath[originalPath.Count - 1].transform.position);
                originalPath.RemoveAt(originalPath.Count - 1);
            }*/
        
            //original = pathOriginalPositionNode;
        
            return pathNode.Select(x => x.Position).Reverse().ToList();
        }


        public Pathfinder SetGraph(Graph graph)
        {
            m_nodeGraph = graph;
            return this;
        }
    }
}
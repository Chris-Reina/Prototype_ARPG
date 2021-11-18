using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DoaT.AI
{
    public class Node : MonoBehaviour, IComparable<Node>, IWeighted
    {
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private float _radius;
        [SerializeField] private float _neighbourSearchRadius;

        public bool isBlocked;
        public int pathNumber;

        public float gCost;  //Node distance from start

        public float hCost;  //distance to finish

        public double Priority => FCost;
        public float Weight => FCost;

        public List<NodeNeighbour> neighbours = new List<NodeNeighbour>();
        
        public Node previousNode;
        public Vector3 Position => transform.position;


        public float Radius { get { return _radius; } set { _radius = value; _collider.radius = value; } }
        public float FCost { get { return gCost + hCost; } }

        private void Awake()
        {
            if(_collider == null)
                _collider = GetComponent<SphereCollider>();

            if(neighbours.Count == 0)
                CalculateNeighbours();
        }
        
        public void Reset()
        {
            gCost = float.MaxValue;
            hCost = 0;
            previousNode = default;
            pathNumber = -1;
        }
        
        public Node SetNeighbourSearchRadius(float radius)
        {
            _neighbourSearchRadius = radius;
            return this;
        }
        public void SetBlockState(LayerMask blockMask)
        {
            var temp = Physics.OverlapSphere(transform.position, _radius, blockMask);

            isBlocked = temp.Length > 0;
        }

        public void CalculateNeighbours()
        {
            var tempR = Physics.OverlapBox(transform.position,
                new Vector3(_neighbourSearchRadius, _neighbourSearchRadius, _neighbourSearchRadius), transform.rotation,
                LayersUtility.NodeMask, QueryTriggerInteraction.Collide);

            var nodes = tempR.Select(n => n.gameObject.GetComponent<Node>())
                                        .Where(n => n != this)
                                        .ToList();

            foreach (var node in from node in nodes ////IA2-P3 Where / OrderBy / Select / Take  (Raider refactorizo la funcion que teniamos pero el LINQ era tal y como está)
                let ray = new Ray(transform.position, (node.transform.position - transform.position).normalized)
                let distance = Vector3.Distance(transform.position, node.transform.position) + _radius //- 0.0001f
                let collisions = Physics.RaycastAll(ray, distance, LayersUtility.NodeNeighbourCheck, QueryTriggerInteraction.Collide)
                let check = collisions
                    .Where(n => n.transform != transform)
                    .OrderBy(n => Vector3.Distance(transform.position, n.point))
                    .Select(n => n.transform.GetComponent<Node>())
                    .Take(1)
                where check != null
                select node)
            {
                neighbours.Add(new NodeNeighbour(node, Vector3.Distance(transform.position, node.transform.position)));
            }
        }

        public int CompareTo(Node other)
        {
            if (Priority  < other.Priority) return -1;
            return Priority  > other.Priority ? 1 : 0;
        }
        
        /*private void DrawIcon(GameObject gameObject, int idx)
        {
            var largeIcons = GetTextures("sv_label_", string.Empty, 0, 8);
            var icon = largeIcons[idx];
            var egu = typeof(EditorGUIUtility);
            var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            var args = new object[] { gameObject, icon.image };
            var setIcon = egu.GetMethod("SetIconForObject", flags, null, new Type[]{typeof(UnityEngine.Object), typeof(Texture2D)}, null);
            setIcon?.Invoke(null, args);
        }
        private GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
        {
            GUIContent[] array = new GUIContent[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = EditorGUIUtility.IconContent(baseName + (startIndex + i) + postFix);
            }
            return array;
        }*/
    }
}


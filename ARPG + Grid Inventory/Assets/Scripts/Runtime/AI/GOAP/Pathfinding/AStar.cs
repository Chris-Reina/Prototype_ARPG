using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DoaT;
using Debug = UnityEngine.Debug;

public class AStar<T>
{
    public event Action<IEnumerable<T>> OnPathCompleted;
    private readonly float _maxCalculationTime;

    public AStar(float maxCalculationTime)
    {
        _maxCalculationTime = maxCalculationTime;
    }

    public IEnumerator Run(T                                        start,
                              Func<T, bool>                         isGoal,
                              Func<T, IEnumerable<WeightedNode<T>>> explode,
                              Func<T, float>                        getHeuristic) {
        
        //var queue     = new APriorityQueue<T>();
        var queue     = new PriorityQueue<WeightedNode<T>>();
        var distances = new Dictionary<T, float>();
        var parents   = new Dictionary<T, T>();
        var visited   = new HashSet<T>();

        distances[start] = 0;
        queue.Enqueue(new WeightedNode<T>(start, 0));
        
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        while (!queue.IsEmpty)
        {
            if (stopWatch.ElapsedMilliseconds > _maxCalculationTime)
            {
                yield return new WaitForEndOfFrame();
                stopWatch.Restart();
            }
            
            var dequeued = queue.Dequeue();
            visited.Add(dequeued.Element);

            if (isGoal(dequeued.Element))
            {
                OnPathCompleted?.Invoke(CommonUtils.CreatePath(parents, dequeued.Element));
                yield break;
            }

            var toEnqueue = explode(dequeued.Element);

            foreach (var transition in toEnqueue) {
                var neighbour                   = transition.Element;
                var neighbourToDequeuedDistance = transition.Weight;

                var startToNeighbourDistance =
                    distances.ContainsKey(neighbour) ? distances[neighbour] : float.MaxValue;
                var startToDequeuedDistance = distances[dequeued.Element];

                var newDistance = startToDequeuedDistance + neighbourToDequeuedDistance;

                if (!visited.Contains(neighbour) && startToNeighbourDistance > newDistance) {
                    distances[neighbour] = newDistance;
                    parents[neighbour]   = dequeued.Element;

                    queue.Enqueue(new WeightedNode<T>(neighbour, newDistance + getHeuristic(neighbour)));
                }
            }
            
            if (queue.IsEmpty)
            {
                OnPathCompleted?.Invoke(CommonUtils.CreatePath(parents, dequeued.Element));
                yield break;
            }
        }
    }

}
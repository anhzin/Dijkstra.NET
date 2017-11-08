﻿using System;
using Dijkstra.NET.Contract;
using Dijkstra.NET.Model;
using Dijkstra.NET.Utility;

namespace Dijkstra.NET.ShortestPath
{
    public class Dijkstra<T, TEdgeCustom> where TEdgeCustom : IEquatable<TEdgeCustom>
    {
        protected readonly IGraph<T, TEdgeCustom> Graph;

        public Dijkstra(IGraph<T, TEdgeCustom> graph)
        {
            Graph = graph;
        }

        /// <summary>
        /// Get path from @from to @to
        /// </summary>
        /// <param name="from">Start node</param>
        /// <param name="to">End node</param>
        /// <returns>Value with path</returns>
        public virtual IShortestPathResult Process(uint from, uint to)
        {
            var result = new DijkstraResult(from, to);

            var q = new PriorityQueue<INode<T, TEdgeCustom>, T, TEdgeCustom>(Graph.Count);

            q.Enqueue(Graph[from], 0);

            while (q.Count > 0)
            {
                INode<T, TEdgeCustom> u = q.Dequeue();

                if (u.Key == to)
                {
                    result.Distance = u.Distance;
                    break;
                }

                for (int i = 0; i < u.Children.Count; i++)
                {
                    Edge<T, TEdgeCustom> e = u.Children[i];

                    if (e.Node.Distance > u.Distance + e.Cost)
                    {
                        int distance = u.Distance + e.Cost;

                        if (q.Contains(e.Node))
                        {
                            q.UpdatePriority(e.Node, distance);
                        }
                        else
                        {
                            q.Enqueue(e.Node, distance);
                        }

                        result.Path[e.Node.Key] = u.Key;
                    }
                }
            }

            return result;
        }
    }
}

using System.Collections.Generic;

namespace Graphs
{
    public static class GraphAlgorithms
    {
        public static IEnumerable<int> BFS(this IGraph graph, int startVertex)
        {
            var visited = new bool[graph.Count];

            var queue = new Queue<int>();

            visited[startVertex] = true;
            queue.Enqueue(startVertex);

            while (queue.Count > 0)
            {
                int currentVertex = queue.Dequeue();

                yield return currentVertex;

                foreach (var edge in graph.GetNeighbors(currentVertex))
                {
                    if (!visited[edge.Target])
                    {
                        visited[edge.Target] = true;
                        queue.Enqueue(edge.Target);
                    }
                }
            }
        }
        public static IEnumerable<int> DFS(this IGraph graph, int startVertex)
        {
            var visited = new bool[graph.Count];
            var stack = new Stack<int>();

            stack.Push(startVertex);

            while (stack.Count > 0)
            {
                int current = stack.Pop();

                if (!visited[current])
                {
                    visited[current] = true;
                    yield return current;

                    foreach (var edge in graph.GetNeighbors(current))
                    {
                        if (!visited[edge.Target])
                        {
                            stack.Push(edge.Target);
                        }
                    }
                }
            }
        }
        public static (int[] distances, int[] previous) Dijkstra(this IGraph graph, int startVertex)
        {
            int n = graph.Count;

            var distances = new int[n];
            var previous = new int[n];
            var unvisited = new List<int>();

            for (int i = 0; i < n; i++)
            {
                distances[i] = int.MaxValue;
                previous[i] = -1;
                unvisited.Add(i);
            }
            distances[startVertex] = 0;

            while (unvisited.Count > 0)
            {
                int current = -1;
                int minDistance = int.MaxValue;

                foreach (int node in unvisited)
                {
                    if (distances[node] < minDistance)
                    {
                        minDistance = distances[node];
                        current = node;
                    }
                }

                if (current == -1 || minDistance == int.MaxValue)
                {
                    break;
                }

                unvisited.Remove(current);

                foreach (var edge in graph.GetNeighbors(current))
                {
                    int neighbor = edge.Target;
                    int weight = edge.Weight;

                    int newDist = distances[current] + weight;

                    if (newDist < distances[neighbor])
                    {
                        distances[neighbor] = newDist;
                        previous[neighbor] = current;
                    }
                }
            }

            return (distances, previous);
        }
    }
}
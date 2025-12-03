using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    public interface IGraph
    {
        int Count { get; }
        bool IsDirected { get; }
        bool IsWeighted { get; }
        IEnumerable<IEdge> GetNeighbors(int index);
    }
    public class AdjacencyMatrixGraph : IGraph
    {
        private readonly int[,] matrix;
        public int Count { get; }
        public bool IsDirected { get; private set; }
        public bool IsWeighted { get; private set; }
        public AdjacencyMatrixGraph(int[,] data)
        {
            matrix = data;
            Count = matrix.GetLength(0);

            IsDirected = false;
            bool found = false;
            for (int i = 0; i < Count && !found; i++)
            {
                for (int j = 0; j < Count; j++)
                {
                    if (matrix[j, i] != matrix[i, j])
                    {
                        IsDirected = true;
                        found = true;
                    }
                }
            }

            found = false;
            IsWeighted = false;
            for (int i = 0; i < Count && !found; i++)
            {
                for (int j = 0; j < Count; j++)
                {
                    if (matrix[i, j] > 1)
                    {
                        IsWeighted = true;
                        found = true;
                    }
                }
            }

        }

        public IEnumerable<IEdge> GetNeighbors(int index)
        {
            for (int i = 0; i < Count; i++)
            {
                if (matrix[index, i] != 0)
                {
                    yield return new MatrixEdge(i, matrix[index, i]);
                }
            }
        }

        private readonly struct MatrixEdge : IEdge
        {
            public int Target { get; }
            public int Weight { get; }

            public MatrixEdge(int target, int weight)
            {
                Target = target;
                Weight = weight;
            }
        }
    }
}


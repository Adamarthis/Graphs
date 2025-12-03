using System;
using System.Text;
using Graphs;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        //                  0    1    2    3    4    5    6    7    8    9    10 
        string[] nodes = { "S", "W", "G", "P", "V", "D", "K", "Z", "X", "B", "T" };

        int n = nodes.Length;
        int[,] data = new int[n, n];

        void Add(int u, int v, int w)
        {
            data[u, v] = w;
        }

        // S (0) -> ...
        Add(0, 1, 4); // S -> W
        Add(0, 2, 3); // S -> G

        // W (1) -> ...
        Add(1, 3, 1); // W -> P
        Add(1, 4, 3); // W -> V

        // G (2) -> ...
        Add(2, 4, 6); // G -> V

        // V (4) -> ...
        Add(4, 5, 2); // V -> D
        Add(4, 6, 10);// V -> K

        // D (5) -> ...
        Add(5, 7, 3); // D -> Z

        // Z (7) -> ...
        Add(7, 8, 1); // Z -> X
        Add(7, 9, 2); // Z -> B

        // X (8) -> ...
        Add(8, 9, 7); // X -> B

        // B (9) -> ...
        Add(9, 10, 5); // B -> T

        var graph = new AdjacencyMatrixGraph(data);

        Console.WriteLine($"Кількість вершин: {graph.Count}");
        Console.WriteLine($"Орієнтований: {(graph.IsDirected ? "Так" : "Ні")}");
        Console.WriteLine($"Зважений: {(graph.IsWeighted ? "Так" : "Ні")}");
        Console.WriteLine();

        Console.WriteLine("Матриця суміжності:");
        PrintMatrix(data, nodes);
        Console.WriteLine();

        Console.WriteLine("=== Результат BFS (Пошук в ширину) ===");
        Console.Write("Порядок обходу: ");

        int startNodeIndex = 0; 
        bool isFirst = true;

        foreach (var nodeIndex in graph.BFS(startNodeIndex))
        {
            if (!isFirst) Console.Write(" -> ");
            Console.Write(nodes[nodeIndex]);
            isFirst = false;
        }
        Console.WriteLine();
        Console.WriteLine(new string('-', 30));
        
        Console.WriteLine();
        Console.WriteLine("=== Результат DFS (Пошук в глибину) ===");
        Console.Write("Порядок обходу: ");

        isFirst = true;

        foreach (var nodeIndex in graph.DFS(0))
        {
            if (!isFirst) Console.Write(" -> ");
            Console.Write(nodes[nodeIndex]);
            isFirst = false;
        }
        Console.WriteLine();
        Console.WriteLine(new string('-', 30));
        Console.WriteLine();
        Console.WriteLine("=== Результат Алгоритму Дейкстри ===");

        var (distances, previous) = graph.Dijkstra(startNodeIndex);

        Console.WriteLine("Найкоротші шляхи від S:");

        for (int i = 0; i < n; i++)
        {
            if (i == startNodeIndex) continue;

            if (distances[i] != int.MaxValue)
            {
                string path = ReconstructPath(previous, i, nodes);
                Console.WriteLine($"До {nodes[i]}: {distances[i],-3} (шлях: {path})");
            }
            else
            {
                Console.WriteLine($"До {nodes[i]}: недосяжна");
            }
        }
        Console.WriteLine(new string('-', 30));
    }

    static string ReconstructPath(int[] previous, int end, string[] labels)
    {
        var pathList = new List<string>();
        int current = end;

        while (current != -1)
        {
            pathList.Add(labels[current]);
            current = previous[current];
        }

        pathList.Reverse();
        return string.Join(" -> ", pathList);
    }
    static void PrintMatrix(int[,] matrix, string[] labels)
    {
        int n = matrix.GetLength(0);

        Console.Write("  ");
        for (int i = 0; i < n; i++) Console.Write($"{labels[i],3}");
        Console.WriteLine();

        for (int i = 0; i < n; i++)
        {
            Console.Write($"{labels[i],2}"); 
            for (int j = 0; j < n; j++)
            {
                int val = matrix[i, j];
                string s = (val == 0) ? "." : val.ToString();
                Console.Write($"{s,3}");
            }
            Console.WriteLine();
        }
    }
}
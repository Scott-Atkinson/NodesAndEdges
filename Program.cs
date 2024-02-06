public class Node
{
    public string Name { get; set; }
    public Dictionary<Node, int> Edges { get; private set; }

    public Node(string name)
    {
        Name = name;
        Edges = new Dictionary<Node, int>();
    }

    public void AddEdge(Node node, int distance)
    {
        Edges[node] = distance;
    }
}

public class Graph
{
    public Dictionary<string, Node> Nodes { get; private set; }

    public Graph()
    {
        Nodes = new Dictionary<string, Node>();
    }

    public void AddNode(string name)
    {
        Nodes[name] = new Node(name);
    }

    public void AddEdge(string from, string to, int distance)
    {
        Nodes[from].AddEdge(Nodes[to], distance);
    }

    public List<string> ShortestPath(string start, string end)
    {
        var previous = new Dictionary<Node, Node>();
        var distances = new Dictionary<Node, int>();
        var nodes = new List<Node>();

        Node source = Nodes[start];
        Node target = Nodes[end];

        foreach (var node in Nodes.Values)
        {
            if (node == source)
            {
                distances[node] = 0;
            }
            else
            {
                distances[node] = int.MaxValue;
            }

            nodes.Add(node);
        }

        while (nodes.Count > 0)
        {
            nodes.Sort((x, y) => distances[x] - distances[y]);
            Node smallest = nodes[0];
            nodes.Remove(smallest);

            if (smallest == target)
            {
                var path = new List<string>();
                while (previous.ContainsKey(smallest))
                {
                    path.Add(smallest.Name);
                    smallest = previous[smallest];
                }

                path.Add(source.Name);
                path.Reverse();
                return path;
            }

            if (distances[smallest] == int.MaxValue)
            {
                break;
            }

            foreach (var neighbor in smallest.Edges)
            {
                int alt = distances[smallest] + neighbor.Value;
                if (alt < distances[neighbor.Key])
                {
                    distances[neighbor.Key] = alt;
                    previous[neighbor.Key] = smallest;
                }
            }
        }

        return new List<string>();
    }
}

public class Program
{
    public static void Main()
    {
        Graph graph = new Graph();

        foreach (char nodeName in new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' })
        {
            graph.AddNode(nodeName.ToString());
        }

        graph.AddEdge("A", "B", 4);
        graph.AddEdge("B", "A", 4);
        graph.AddEdge("A", "C", 6);
        graph.AddEdge("C", "A", 6);
        graph.AddEdge("C", "D", 8);
        graph.AddEdge("D", "C", 8);
        graph.AddEdge("D", "E", 4);
        graph.AddEdge("E", "D", 4);
        graph.AddEdge("D", "G", 1);
        graph.AddEdge("G", "D", 1);
        graph.AddEdge("G", "I", 5);
        graph.AddEdge("I", "G", 5);
        graph.AddEdge("E", "I", 8);
        graph.AddEdge("I", "E", 8);
        graph.AddEdge("E", "B", 2);
        graph.AddEdge("E", "F", 3);
        graph.AddEdge("F", "E", 3);
        graph.AddEdge("F", "G", 4);
        graph.AddEdge("G", "F", 4);
        graph.AddEdge("F", "H", 6);
        graph.AddEdge("H", "F", 6);
        graph.AddEdge("H", "G", 5);
        graph.AddEdge("G", "H", 5);

        List<string> shortestPath = graph.ShortestPath("A", "D");

        Console.WriteLine($"Shortest path from A to D: {string.Join(" -> ", shortestPath)}");
    }
}

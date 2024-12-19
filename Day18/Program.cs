using System.Numerics;

string inputFile = "input.txt";
// inputFile = "test.txt";

int width = 70, height = 70;
if (inputFile == "test.txt")
{
    width = 6;
    height = 6;
}

var start = new Vector2(0, 0);
var end = new Vector2(width, height);

HashSet<Vector2> bytes = new();

int maxBytes = 1024;
if (inputFile == "test.txt")
    maxBytes = 12;

var directions = new Vector2[4] {
    new Vector2(1, 0), // right
    new Vector2(0, 1), // down
    new Vector2(-1, 0), // left
    new Vector2(0, -1), // up
};

foreach (string line in File.ReadAllLines(inputFile))
{
    int[] nums = line.Split(',').Select(int.Parse).ToArray();
    var vec = new Vector2(nums[0], nums[1]);
    bytes.Add(vec);

    maxBytes--;
    if (maxBytes == 0)
    {
        Console.WriteLine($"Part 1: {Dijkstra(start, end, bytes)}");
    }
    else if (maxBytes < 0)
    {
        if (Dijkstra(start, end, bytes) == -1)
        {
            Console.WriteLine($"Part 2: {nums[0]},{nums[1]}");
            break;
        }

    }
}

int Dijkstra(Vector2 start, Vector2 end, HashSet<Vector2> bytes)
{
    PriorityQueue<Vector2, int> pq = new();
    Dictionary<Vector2, Vector2> parent = new();
    HashSet<Vector2> visited = new();
    Dictionary<Vector2, int> cost = new();

    pq.Enqueue(start, 0);
    cost[start] = 0;

    while (pq.Count > 0)
    {
        Vector2 current = pq.Dequeue();
        Visualize(bytes, cost, current);
        if (current == end) break;

        if (visited.Contains(current)) continue;
        visited.Add(current);

        foreach (Vector2 adj in GetAdjNodes(current))
        {
            if (!cost.ContainsKey(adj)) cost[adj] = int.MaxValue;
            int newCost = cost[current] + 1;

            if (newCost < cost[adj])
            {
                cost[adj] = newCost;
                parent[adj] = current;
                pq.Enqueue(adj, newCost);
            }
        }
    }
    try
    {
        return cost[end];
    }
    catch
    {
        return -1;
    }
}

void Visualize(HashSet<Vector2> bytes, Dictionary<Vector2, int> cost,
               Vector2? current = null, List<Vector2>? best = null)
{
    if (inputFile == "input.txt") return;
    Console.WriteLine();
    for (int y = 0; y <= height; y++)
    {
        for (int x = 0; x <= width; x++)
        {
            var vec = new Vector2(x, y);
            if (current is not null && vec == current)
                Console.BackgroundColor = ConsoleColor.Magenta;
            if (bytes.Contains(vec))
            {
                Console.Write("[###]");
            }
            else if (cost.ContainsKey(vec))
            {
                Console.Write($"[{cost[vec].ToString("D3")}]");
            }
            else
            {
                Console.Write("[...]");
            }
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    Console.ReadKey();
}

List<Vector2> GetAdjNodes(Vector2 node)
{
    List<Vector2> adjNodes = new();
    foreach (Vector2 dir in directions)
    {
        Vector2 newNode = node + dir;
        if (Valid((int)newNode.X, (int)newNode.Y) && !bytes.Contains(newNode))
        {
            adjNodes.Add(newNode);
        }
    }
    return adjNodes;
}

bool Valid(int x, int y)
{
    return x >= 0 && x <= width && y >= 0 && y <= height;
}
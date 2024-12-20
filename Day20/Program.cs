using System.Numerics;
using System.Runtime.InteropServices;

string inputFile = "input.txt";
// inputFile = "test.txt";

Vector2 start = Vector2.Zero;
Vector2 end = Vector2.Zero;

HashSet<Vector2> walls = new();

var directions = new Vector2[4] {
    new Vector2(1, 0), // right
    new Vector2(0, 1), // down
    new Vector2(-1, 0), // left
    new Vector2(0, -1), // up
};

var lines = File.ReadAllLines(inputFile);

for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines[y].Length; x++)
    {
        if (lines[y][x] == '#') walls.Add(new Vector2(x, y));

        if (lines[y][x] == 'S') start = new Vector2(x, y);
        if (lines[y][x] == 'E') end = new Vector2(x, y);
    }
}

int height = lines.Length;
int width = lines[0].Length;

int noCheatCost = Dijkstra(start, end, walls, false);

Console.WriteLine($"No cheat cost: {noCheatCost}");

int goodCheats = 0;
int threshold = 100;
if (inputFile == "test.txt") threshold = 20;

foreach (Vector2 wall in walls)
{
    int cheatCost = Dijkstra(start, end, walls, false, noCheatCost - threshold, wall);
    // if (cheatCost >= 0) Console.WriteLine($"{cheatCost}");
    if (cheatCost > 0 && noCheatCost - cheatCost >= threshold) goodCheats++;
}

Console.WriteLine($"{goodCheats} cheats saved at least {threshold} picoseconds");

int Dijkstra(Vector2 start, Vector2 end, HashSet<Vector2> walls, bool viz = true,
             int maxCost = int.MaxValue, Vector2? cheat = null)
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
        if (viz) Visualize(walls, cost, current, cheat);
        if (current == end) break;
        if (visited.Contains(current)) continue;
        visited.Add(current);
        if (cost.ContainsKey(current) && cost[current] > maxCost) break;

        foreach (Vector2 adj in GetAdjNodes(current, cheat))
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

List<Vector2> GetAdjNodes(Vector2 node, Vector2? cheat = null)
{
    List<Vector2> adjNodes = new();
    foreach (Vector2 dir in directions)
    {
        Vector2 newNode = node + dir;
        if (Valid((int)newNode.X, (int)newNode.Y))
        {
            if (walls.Contains(newNode) && newNode != cheat) continue;
            adjNodes.Add(newNode);
        }
    }
    return adjNodes;
}

bool Valid(int x, int y)
{
    return x >= 0 && x <= width && y >= 0 && y <= height;
}

void Visualize(HashSet<Vector2> grid, Dictionary<Vector2, int> cost,
                Vector2? current = null, Vector2? cheat = null, List<Vector2>? best = null)
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
            if (cheat is not null && cheat == vec)
                Console.BackgroundColor = ConsoleColor.Yellow;
            if (vec == start) Console.BackgroundColor = ConsoleColor.Blue;
            if (vec == end) Console.BackgroundColor = ConsoleColor.Green;
            if (grid.Contains(vec) && cheat != vec)
            {
                Console.Write("[##]");
            }
            else if (cost.ContainsKey(vec))
            {
                Console.Write($"[{cost[vec].ToString("D2")}]");
            }
            else
            {
                Console.Write("[..]");
            }
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    Console.ReadKey();
}


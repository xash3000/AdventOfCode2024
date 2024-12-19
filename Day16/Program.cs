using System.Numerics;

string inputFile = "input.txt";
// inputFile = "test.txt";

string[] lines = File.ReadAllLines(inputFile);

var grid = new List<List<char>>();

Vector2 start = Vector2.Zero;
Vector2 end = Vector2.Zero;

int i = 0;
foreach (string line in lines)
{
    if (line.Contains("S"))
    {
        start = new Vector2(line.IndexOf("S"), i);
    }
    else if (line.Contains("E"))
    {
        end = new Vector2(line.IndexOf("E"), i);
    }
    grid.Add(line.ToList());
    i++;
}

int width = grid[0].Count;
int height = grid.Count;

var directions = new List<Vector2>
{
    new Vector2(0, 1),
    new Vector2(0, -1),
    new Vector2(1, 0),
    new Vector2(-1, 0)
};

var pq = new PriorityQueue<Node, int>();
var cost = new Dictionary<Vector2, int>();
var parents = new Dictionary<Vector2, List<Vector2>>();

var startNode = new Node(start, directions[2]);

pq.Enqueue(startNode, 0);
cost[start] = 0;

while (pq.Count > 0)
{
    Node current = pq.Dequeue();
    // Visualize(null, current.Pos); Console.ReadKey();
    if (current.Pos == end) break;

    foreach (Node adj in GetAdjNodes(current.Pos))
    {
        if (!cost.ContainsKey(adj.Pos)) cost[adj.Pos] = int.MaxValue;

        int newCost = cost[current.Pos] + Weight(current, adj);
        if (!parents.ContainsKey(adj.Pos)) parents[adj.Pos] = new List<Vector2>();

        if (newCost < cost[adj.Pos])
        {
            cost[adj.Pos] = newCost;
            pq.Enqueue(adj, newCost);

            parents[adj.Pos].Clear(); // we found a better path, clear the old ones
            parents[adj.Pos].Add(current.Pos);
        }
        else if (newCost == cost[adj.Pos] || Math.Abs(newCost - cost[adj.Pos]) == 1000)
        {
            parents[adj.Pos].Add(current.Pos);
            cost[adj.Pos] = newCost;
            pq.Enqueue(adj, newCost);
        }
    }
}

var bestNodes = new HashSet<Vector2>();

BacktrackPaths(ref bestNodes, end);

var part1 = new Part1();
part1.Run();

Console.WriteLine($"Part 2: {bestNodes.Count}");

Visualize(bestNodes);

void Visualize(HashSet<Vector2>? best = null, Vector2? current = null)
{
    if (inputFile == "input.txt") return;
    Console.WriteLine();
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[y][x] == '#')
            {
                Console.Write("[####]");
            }
            else if (best is not null && best.Contains(new Vector2(x, y)))
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write($"[{cost[new Vector2(x, y)].ToString("D4")}]");
                Console.ResetColor();
            }
            else if (cost.ContainsKey(new Vector2(x, y)))
            {
                if (current is not null && current == new Vector2(x, y))
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                }
                Console.Write($"[{cost[new Vector2(x, y)].ToString("D4")}]");
                Console.ResetColor();
            }
            else
            {
                Console.Write("[....]");
            }
        }
        Console.WriteLine();
    }
}

void BacktrackPaths(ref HashSet<Vector2> bestNodes, Vector2 node)
{
    bestNodes.Add(node);

    if (node == start) return;

    foreach (Vector2 parent in parents[node])
    {
        if (!bestNodes.Contains(parent))
            BacktrackPaths(ref bestNodes, parent);
    }
}

bool Valid(Vector2 pos)
{
    return pos.X >= 0 && pos.X < width && pos.Y >= 0 && pos.Y < height;
}

List<Node> GetAdjNodes(Vector2 pos)
{
    var adj = new List<Node>();

    foreach (Vector2 dir in directions)
    {
        Vector2 newPos = pos + dir;
        if (Valid(newPos) && grid[(int)newPos.Y][(int)newPos.X] != '#')
        {
            adj.Add(new Node(newPos, dir));
        }
    }

    return adj;
}

// return 1 if from a to b we go forward, add 1000 to every 90 degree turn
int Weight(Node current, Node adj)
{
    if (current.Direction == adj.Direction) return 1;
    return 1001;
}

class Node
{
    public Vector2 Pos { get; set; }
    public Vector2 Direction { get; set; }

    public Node(Vector2 pos, Vector2 direction)
    {
        Pos = pos;
        Direction = direction;
    }
}
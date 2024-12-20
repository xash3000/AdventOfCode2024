using System.Numerics;
using System.Runtime.CompilerServices;
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

int height = lines.Length - 1;
int width = lines[0].Length - 1;
int minSave = 100, cheatPeriod = 2;
if (inputFile == "test.txt") minSave = 20;

List<Vector2> mainPath = Race(start, end);

Console.WriteLine($"No cheat cost: {mainPath.Count - 1}");

Console.WriteLine($"Part1: Good cheats count: {FindGoodCheats(mainPath, minSave, cheatPeriod)}");

cheatPeriod = 20;
Console.WriteLine($"Part 2: Good cheats count: {FindGoodCheats(mainPath, minSave, cheatPeriod)}");

List<Vector2> Race(Vector2 start, Vector2 end)
{
    Vector2 current = start;
    List<Vector2> mainPath = new();
    mainPath.Add(current);
    while (current != end)
    {
        foreach (Vector2 dir in directions)
        {
            Vector2 newNode = current + dir;
            if (Valid(newNode) && !mainPath.Contains(newNode) && !walls.Contains(newNode))
            {
                current = newNode;
                mainPath.Add(newNode);
                break;
            }
        }
    }
    return mainPath;
}

int FindGoodCheats(List<Vector2> path, int minSave, int cheatPeriod)
{
    int goodCheats = 0;
    for (int i = 0; i < mainPath.Count; i++)
    {
        for (int j = i + 1; j < mainPath.Count; j++)
        {
            int dist = ManhattanDistance(mainPath[i], mainPath[j]);
            if (dist > cheatPeriod) continue;
            int save = j - i - dist;
            if (save >= minSave) goodCheats++;
        }
    }
    return goodCheats;
}

bool Valid(Vector2 node)
{
    int x = (int)node.X;
    int y = (int)node.Y;
    return x >= 0 && x <= width && y >= 0 && y <= height;
}

int ManhattanDistance(Vector2 start, Vector2 end)
{
    return (int)Math.Abs(start.X - end.X) + (int)Math.Abs(start.Y - end.Y);
}
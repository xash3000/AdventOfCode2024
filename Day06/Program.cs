string inputFile = "input.txt";
// inputFile = "test.txt";

List<List<char>> grid = new();

int maxX = 0, maxY = 0;
int guardX = 0, guardY = 0;
int guardInitX = 0, guardInitY = 0;

foreach (var line in File.ReadLines(inputFile))
{
    if (line.Contains('^'))
    {
        guardY = guardInitY = maxY;
        guardX = guardInitX = line.IndexOf('^');
    }
    maxY++;
    grid.Add(line.ToList());
}

maxX = grid[0].Count;

HashSet<Tuple<int, int>> visistedPoints = new();
int fwdX = 0, fwdY = -1;

while (true)
{
    visistedPoints.Add(Tuple.Create(guardX, guardY));
    if (!Valid(guardX + fwdX, guardY + fwdY)) break;
    if (grid[guardY + fwdY][guardX + fwdX] == '#') Rotate();
    guardX += fwdX;
    guardY += fwdY;
}

Console.WriteLine($"Part 1: {visistedPoints.Count}");

// Part 2
int obstacles = 0;
int c = 0;
foreach (var point in visistedPoints)
{
    Console.WriteLine($"{++c}/{visistedPoints.Count}");
    Console.WriteLine($"Obstacles: {obstacles}");
    Console.WriteLine($"Point: {point.Item2}, {point.Item1}");
    Console.WriteLine();
    if (point.Item1 == guardInitX && point.Item2 == guardInitY) continue;
    int px = point.Item1;
    int py = point.Item2;
    grid[py][px] = '#';
    if (Loops()) obstacles++;
    grid[py][px] = '.';
}

Console.WriteLine($"Part 2: {obstacles}");

bool Loops()
{
    int x = guardInitX;
    int y = guardInitY;

    fwdX = 0; fwdY = -1;

    HashSet<Tuple<int, int>> visited = new();
    Dictionary<Tuple<int, int>, Tuple<int, int>> direction = new();
    while (true)
    {
        if (!Valid(x + fwdX, y + fwdY)) return false;
        while (grid[y + fwdY][x + fwdX] == '#') Rotate();
        if (visited.Contains(Tuple.Create(x, y)) && direction[Tuple.Create(x, y)].Equals(Tuple.Create(fwdX, fwdY))) return true;
        visited.Add(Tuple.Create(x, y));
        direction[Tuple.Create(x, y)] = Tuple.Create(fwdX, fwdY);
        x += fwdX;
        y += fwdY;
    }
}

void Rotate()
{
    if (fwdX == 0)
    {
        fwdX = -fwdY;
        fwdY = 0;
    }
    else
    { // fwd == 0
        fwdY = fwdX;
        fwdX = 0;
    }
}

bool Valid(int x, int y)
{
    return 0 <= x && x < maxX && 0 <= y && y < maxY;
}
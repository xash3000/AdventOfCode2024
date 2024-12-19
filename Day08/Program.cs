string inputFile = "input.txt";
// inputFile = "test.txt";

string[] lines = File.ReadAllLines(inputFile);

List<List<char>> grid = new();
Dictionary<char, List<Tuple<int, int>>> antennas = new();

for (int i = 0; i < lines.Length; i++)
{
    grid.Add(new List<char>());
    for (int j = 0; j < lines[i].Length; j++)
    {
        grid[i].Add(lines[i][j]);
        if (lines[i][j] != '.' && lines[i][j] != '#')
        {
            if (!antennas.ContainsKey(lines[i][j])) antennas[lines[i][j]] = new();
            antennas[lines[i][j]].Add(new Tuple<int, int>(i, j));
        }
    }
}

int width = grid[0].Count;
int height = grid.Count;

HashSet<Tuple<int, int>> antinodes = new();

foreach (var kvp in antennas)
{
    char freq = kvp.Key;
    List<Tuple<int, int>> points = kvp.Value;
    for (int i = 0; i < points.Count; i++)
    {
        for (int j = 0; j < points.Count; j++)
        {
            if (i == j) continue;
            Tuple<int, int> p1 = points[i];
            Tuple<int, int> p2 = points[j];
            float slope = CalcSlope(p1, p2);
            FindAntinodes(p1, p2, slope);
        }
    }
}

Console.WriteLine($"Part 1: {antinodes.Count}");

// Part 2
antinodes.Clear();

foreach (var kvp in antennas)
{
    char freq = kvp.Key;
    List<Tuple<int, int>> points = kvp.Value;
    for (int i = 0; i < points.Count; i++)
    {
        for (int j = 0; j < points.Count; j++)
        {
            if (i == j) continue;
            Tuple<int, int> p1 = points[i];
            Tuple<int, int> p2 = points[j];
            float slope = CalcSlope(p1, p2);
            FindAntinodes(p1, p2, slope, true);
        }
    }
}

Console.WriteLine($"Part 2: {antinodes.Count}");

float CalcSlope(Tuple<int, int> p1, Tuple<int, int> p2)
{
    if (p2.Item2 == p1.Item2)
    {
        return float.PositiveInfinity;
    }
    return (float)(p1.Item1 - p2.Item1) / (p1.Item2 - p2.Item2);
}

bool Valid(int i, int j)
{
    return i >= 0 && i < height && j >= 0 && j < width;
}

int Distance(Tuple<int, int> p1, Tuple<int, int> p2)
{
    return Math.Abs(p1.Item1 - p2.Item1) + Math.Abs(p1.Item2 - p2.Item2);
}

void FindAntinodes(Tuple<int, int> p1, Tuple<int, int> p2, float slope, bool resonant = false)
{
    int i1 = p1.Item1;
    int j1 = p1.Item2;
    int i2 = p2.Item1;
    int j2 = p2.Item2;

    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            var p = new Tuple<int, int>(i, j);
            if (Valid(i, j) && CalcSlope(p, p1) == slope && CalcSlope(p, p2) == slope)
            {
                int d1 = Distance(p, p1);
                int d2 = Distance(p, p2);
                if (resonant || d1 == 2 * d2 || d2 == 2 * d1)
                {
                    antinodes.Add(p);
                }
            }
        }
    }
    antinodes.Add(p1);
    antinodes.Add(p2);
}
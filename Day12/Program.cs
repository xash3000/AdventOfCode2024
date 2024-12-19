string inputFile = "input.txt";
// inputFile = "test.txt";

List<List<char>> grid = new();
var lines = File.ReadAllLines(inputFile);

for (int i = 0; i < lines.Length; i++)
{
    grid.Add(new List<char>());
    for (int j = 0; j < lines[i].Length; j++)
    {
        grid[i].Add(lines[i][j]);
    }
}

int width = grid[0].Count;
int height = grid.Count;

// clockwise order up, right, down, left
int[] di = { -1, 0, 1, 0 };
int[] dj = { 0, 1, 0, -1 };

HashSet<Tuple<int, int>> visited = new();

int totalPrice = 0;

for (int i = 0; i < grid.Count; i++)
{
    for (int j = 0; j < grid[i].Count; j++)
    {
        if (!visited.Contains(Tuple.Create(i, j)))
        {
            totalPrice += FencingCost(i, j);
        }
    }
}

Console.WriteLine($"Part 1: {totalPrice}");

visited.Clear();
totalPrice = 0;

for (int i = 0; i < grid.Count; i++)
{
    for (int j = 0; j < grid[i].Count; j++)
    {
        if (!visited.Contains(Tuple.Create(i, j)))
        {
            totalPrice += FencingCost(i, j, true);
        }
    }
}

Console.WriteLine($"Part 2: {totalPrice}");

int FencingCost(int i, int j, bool bySides = false)
{
    int area = 0;
    int perimeter = 0;
    int sides = 0;
    char plot = grid[i][j];

    Dictionary<int, List<int>> ups = new();
    Dictionary<int, List<int>> downs = new();
    Dictionary<int, List<int>> lefts = new();
    Dictionary<int, List<int>> rights = new();

    Queue<Tuple<int, int>> queue = new();
    queue.Enqueue(Tuple.Create(i, j));

    while (queue.Count > 0)
    {

        (int ci, int cj) = queue.Dequeue();
        if (visited.Contains(Tuple.Create(ci, cj))) continue;
        area++;

        if (bySides)
        {
            calcSides(ci, cj, plot, ref ups, ref downs, ref lefts, ref rights);
        }
        else
        {
            perimeter += CalcPerimeter(ci, cj, plot);
        }
        visited.Add(Tuple.Create(ci, cj));
        for (int k = 0; k < 4; k++)
        {
            int ni = ci + di[k];
            int nj = cj + dj[k];
            var tup = Tuple.Create(ni, nj);

            if (Valid(ni, nj) && grid[ni][nj] == plot && !visited.Contains(tup))
            {
                queue.Enqueue(tup);
            }
        }
    }
    if (bySides)
    {
        sides += CalcDistinctFences(ups)
                 + CalcDistinctFences(downs)
                 + CalcDistinctFences(lefts)
                 + CalcDistinctFences(rights);

        Console.WriteLine($"Plot: {plot} Area: {area} Sides: {sides}");
        return area * sides;
    }
    return area * perimeter;
}

void calcSides(
    int i, int j, char plot,
    ref Dictionary<int, List<int>> ups, ref Dictionary<int, List<int>> downs,
    ref Dictionary<int, List<int>> lefts, ref Dictionary<int, List<int>> rights
    )
{
    for (int k = 0; k < 4; k++)
    {
        int ni = i + di[k];
        int nj = j + dj[k];
        if (!Valid(ni, nj) || grid[ni][nj] != plot)
        {
            if (k == 0)
            {
                if (!ups.ContainsKey(ni)) ups[ni] = new List<int>();
                ups[ni].Add(nj);
            }
            else if (k == 1)
            {
                if (!rights.ContainsKey(nj)) rights[nj] = new List<int>();
                rights[nj].Add(ni);
            }
            else if (k == 2)
            {
                if (!downs.ContainsKey(ni)) downs[ni] = new List<int>();
                downs[ni].Add(nj);
            }
            else if (k == 3)
            {
                if (!lefts.ContainsKey(nj)) lefts[nj] = new List<int>();
                lefts[nj].Add(ni);
            }
        }
    }
}

int CalcDistinctFences(Dictionary<int, List<int>> dict)
{
    int result = 0;
    foreach (var item in dict)
    {
        List<int> fences = item.Value;
        fences.Sort();
        for (int i = 0; i < fences.Count - 1; i++)
        {
            if (fences[i] + 1 != fences[i + 1]) result++;
        }
        if (fences.Count > 0) result++; // last fence is always distinct
    }
    return result;
}

int CalcPerimeter(int i, int j, char plot)
{
    int result = 0;
    for (int k = 0; k < 4; k++)
    {
        int ni = i + di[k];
        int nj = j + dj[k];
        if (!Valid(ni, nj) || grid[ni][nj] != plot) result++;
    }
    return result;
}

bool Valid(int x, int y)
{
    return x >= 0 && x < width && y >= 0 && y < height;
}
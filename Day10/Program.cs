string inputFile = "input.txt";
// inputFile = "test.txt";

int maxi = 0, maxj = 0;

var lines = File.ReadAllLines(inputFile);

List<List<char>> map = new();
List<Tuple<int, int>> trailheads = new();

for (int i = 0; i < lines.Length; i++)
{
    map.Add(new List<char>());
    for (int j = 0; j < lines[i].Length; j++)
    {
        map[i].Add(lines[i][j]);
        if (map[i][j] == '0')
        {
            trailheads.Add(new Tuple<int, int>(i, j));
        }
    }
}

maxi = map.Count;
maxj = map[0].Count;

// right, down, left, up
int[] di = { 0, 1, 0, -1 };
int[] dj = { 1, 0, -1, 0 };

// part 1
int result = 0;
foreach (var trailhead in trailheads)
{
    result += CalcScore(trailhead);
}

Console.WriteLine($"Part 1: {result}");

// part 2
result = 0;
foreach (var trailhead in trailheads)
{
    result += CalcScore(trailhead, true);
}

Console.WriteLine($"Part 2: {result}");

int CalcScore(Tuple<int, int> trailhead, bool distinct = false)
{
    int i = trailhead.Item1;
    int j = trailhead.Item2;

    List<Tuple<int, int>> summits = new();

    Queue<Tuple<int, int>> queue = new();

    queue.Enqueue(new Tuple<int, int>(i, j));

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        i = current.Item1;
        j = current.Item2;

        if (map[i][j] == '9')
        {
            summits.Add(new Tuple<int, int>(i, j));
            continue;
        }

        for (int k = 0; k < 4; k++)
        {
            int ni = i + di[k];
            int nj = j + dj[k];

            if (Valid(ni, nj) && NextInPath(map[i][j], map[ni][nj]))
            {
                queue.Enqueue(new Tuple<int, int>(ni, nj));
            }
        }
    }
    if (!distinct) summits = summits.Distinct().ToList();
    return summits.Count;
}

bool NextInPath(char current, char next)
{
    if (next == '.') return false;
    return int.Parse(next.ToString()) == int.Parse(current.ToString()) + 1;
}

bool Valid(int i, int j)
{
    return i >= 0 && i < maxi && j >= 0 && j < maxj;
}
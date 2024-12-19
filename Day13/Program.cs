string inputFile = "input.txt";
// inputFile = "test.txt";

var lines = File.ReadAllLines(inputFile);

var problems = lines
    .Where(line => !string.IsNullOrWhiteSpace(line))
    .Select((line, index) => new { line, index })
    .GroupBy(x => x.index / 3)
    .Select(group => group.Select(x => x.line).ToArray())
    .Select(ParseProblem)
    .ToList();

long result = 0;
foreach (var problem in problems)
{
    result += FindCostToTarget(problem.A, problem.B, problem.Target);
}

Console.WriteLine($"Part 1: {result}");

result = 0;
foreach (var problem in problems)
{
    result += FindCostToTarget(problem.A, problem.B, problem.Target, true);
}

Console.WriteLine($"Part 2: {result}");

(Point A, Point B, Point Target) ParseProblem(string[] problemLines)
{
    var A = ParsePoint(problemLines[0]);
    var B = ParsePoint(problemLines[1]);
    var target = ParsePoint(problemLines[2], isTarget: true);
    return (A, B, target);
}

Point ParsePoint(string line, bool isTarget = false)
{
    var parts = line.Split(new[] { 'X', 'Y', '=', '+', ',' }, StringSplitOptions.RemoveEmptyEntries);
    long x = long.Parse(parts[1]);
    long y = long.Parse(parts[3]);
    return new Point(x, y);
}

long FindCostToTarget(Point A, Point B, Point target, bool error = false)
{
    if (error)
    {
        target.x += 10000000000000;
        target.y += 10000000000000;
    }

    long a = (target.x * B.y - target.y * B.x) / (A.x * B.y - A.y * B.x);
    long b = (target.x - A.x * a) / B.x;

    if (A.x * a + B.x * b != target.x || A.y * a + B.y * b != target.y) return 0;

    if (inputFile == "test.txt") Console.WriteLine($"a: {a}, b: {b}");
    return (long)(3 * a + b);
}

class Point
{
    public long x, y;
    public Point(long x, long y)
    {
        this.x = x;
        this.y = y;
    }
}
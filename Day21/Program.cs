using Cache = System.Collections.Concurrent.ConcurrentDictionary<(char currentKey, char nextKey, int depth), long>;
using Keypad = System.Collections.Generic.Dictionary<char, Vec2>;

string inputFile = "input.txt";
// inputFile = "test.txt";

var numpad = new Keypad()
{
    {'7', new Vec2(0, 0)},
    {'8', new Vec2(1, 0)},
    {'9', new Vec2(2, 0)},
    {'4', new Vec2(0, 1)},
    {'5', new Vec2(1, 1)},
    {'6', new Vec2(2, 1)},
    {'1', new Vec2(0, 2)},
    {'2', new Vec2(1, 2)},
    {'3', new Vec2(2, 2)},
    {'g', new Vec2(0, 3)},
    {'0', new Vec2(1, 3)},
    {'A', new Vec2(2, 3)},
};

var dirpad = new Keypad()
{
    {'g', new Vec2(0, 0)},
    {'^', new Vec2(1, 0)},
    {'A', new Vec2(2, 0)},
    {'<', new Vec2(0, 1)},
    {'v', new Vec2(1, 1)},
    {'>', new Vec2(2, 1)},
};

var lines = File.ReadAllLines(inputFile);

long totalComplexity = 0;
foreach (string code in lines)
{
    totalComplexity += CalculateComplexity(code, 3);
}

Console.WriteLine($"Part 1: {totalComplexity}");

totalComplexity = 0;
foreach (string code in lines)
{
    totalComplexity += CalculateComplexity(code, 26);
}
Console.WriteLine($"Part 2: {totalComplexity}");
long CalculateComplexity(string code, int depth)
{
    Cache cache = new();

    long numericPart = long.Parse(code.Substring(0, 3));
    long totalSequenceLength = FindShortestSequence(code, depth, cache, depth);

    Console.WriteLine($"code={code} comp={numericPart} * {totalSequenceLength}");
    return numericPart * totalSequenceLength;
}

long FindShortestSequence(string code, int depth, Cache cache, int maxDepth)
{
    if (depth == 0) return code.Length;

    var currentKey = 'A';
    var length = 0L;

    foreach (var nextKey in code)
    {
        length += ProcessKey(currentKey, nextKey, depth, cache, maxDepth);
        currentKey = nextKey;
    }
    return length;
}

long ProcessKey(char currentKey, char nextKey, int depth, Cache cache, int maxDepth)
{
    return cache.GetOrAdd((currentKey, nextKey, depth), _ =>
    {
        Keypad keypad = dirpad;
        if (depth == maxDepth) keypad = numpad;

        Vec2 src = keypad[currentKey];
        Vec2 dist = keypad[nextKey];

        string horizKeys = MoveHoriz(src, dist);
        string vertKeys = MoveVert(src, dist);

        var cost = long.MaxValue;

        if (keypad['g'] != new Vec2(src.x, dist.y))
        {
            cost = Math.Min(cost, FindShortestSequence($"{vertKeys}{horizKeys}A", depth - 1, cache, maxDepth));
        }

        if (keypad['g'] != new Vec2(dist.x, src.y))
        {
            cost = Math.Min(cost, FindShortestSequence($"{horizKeys}{vertKeys}A", depth - 1, cache, maxDepth));
        }
        return cost;
    });
}

string MoveHoriz(Vec2 srcVec, Vec2 distVec)
{
    if (srcVec.x == distVec.x) return "";
    int dx = srcVec.x - distVec.x;
    return new string(dx > 0 ? '<' : '>', (int)Math.Abs(dx));
}

string MoveVert(Vec2 srcVec, Vec2 distVec)
{
    if (srcVec.y == distVec.y) return "";
    int dy = srcVec.y - distVec.y;
    return new string(dy > 0 ? '^' : 'v', (int)Math.Abs(dy));
}

record struct Vec2(int x, int y);
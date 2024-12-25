string inputFile = "input.txt";
// inputFile = "test.txt";

var lines = File.ReadAllLines(inputFile);

List<int[]> locks = new();
List<int[]> keys = new();

for (int i = 0; i < lines.Length; i++)
{
    if (string.IsNullOrEmpty(lines[i])) continue;

    int[] block = ParseBlock(lines[i..(i + 7)]);

    if (lines[i].StartsWith('#'))
        locks.Add(block);
    else
        keys.Add(block);
    i += 7;
}

int fitPairs = 0;
foreach (var _lock in locks)
{
    foreach (var key in keys)
    {
        if (Fit(_lock, key)) fitPairs++;
    }
}

Console.WriteLine($"Part 1: {fitPairs}");

int[] ParseBlock(string[] lines)
{
    var block = new int[5];
    Array.Fill(block, -1);
    foreach (var line in lines)
    {
        for (int i = 0; i < 5; i++)
            if (line[i] == '#') block[i]++;
    }
    return block;
}

bool Fit(int[] _lock, int[] key)
{
    for (int i = 0; i < 5; i++)
    {
        if (_lock[i] + key[i] >= 6) return false;
    }
    return true;
}
string inputFile = "input.txt";
// inputFile = "test.txt";

var lines = File.ReadAllLines(inputFile);

Dictionary<char, List<string>> towels = new();
foreach (string pattern in lines[0].Split(", "))
{
    if (!towels.ContainsKey(pattern[0]))
        towels[pattern[0]] = new List<string>();
    towels[pattern[0]].Add(pattern);
}

int possibleDesigns = 0;
long totalWays = 0;

for (int i = 2; i < lines.Length; i++)
{
    string design = lines[i];
    Dictionary<int, long> memo = new();
    long matchResult = CountWays(design, 0, memo);
    if (matchResult > 0) possibleDesigns++;
    totalWays += matchResult;
}

Console.WriteLine($"Part 1: {possibleDesigns}");
Console.WriteLine($"Part 2: {totalWays}");

long CountWays(string design, int currentIndex, Dictionary<int, long> memo)
{
    if (currentIndex == design.Length) return 1;
    if (currentIndex > design.Length) return 0;

    if (memo.ContainsKey(currentIndex)) return memo[currentIndex];

    char currentChar = design[currentIndex];
    if (!towels.ContainsKey(currentChar))
    {
        memo[currentIndex] = 0;
        return 0;
    }

    long ways = 0;
    foreach (string pattern in towels[currentChar])
    {
        if (currentIndex + pattern.Length <= design.Length && design.Substring(currentIndex, pattern.Length) == pattern)
        {
            ways += CountWays(design, currentIndex + pattern.Length, memo);
        }
    }

    memo[currentIndex] = ways;
    return ways;
}
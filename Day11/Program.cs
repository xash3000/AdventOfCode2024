string inputFile = "input.txt";
// inputFile = "test.txt";
bool print = false;
// print = true;

List<string> stones = File.ReadAllText(inputFile).Split(" ").ToList();

Console.WriteLine($"Part 1: {ProcessBlinks(new List<string>(stones), 25)}");
Console.WriteLine($"Part 2: {ProcessBlinks(new List<string>(stones), 75)}");

long ProcessBlinks(List<string> stones, int blinks)
{
    Dictionary<string, long> store = new();

    stones.ForEach(stone =>
    {
        store[stone] = store.ContainsKey(stone) ? store[stone] + 1 : 1;
    });

    List<Tuple<string, long>> newStones = new();

    while (blinks > 0)
    {
        PrintStones(stones);

        newStones.Clear();
        foreach (var kvp in store)
        {
            string stone = kvp.Key;
            long count = kvp.Value;

            if (stone == "0")
            {
                newStones.Add(Tuple.Create("1", count));
                store["0"] -= count;
                if (store["0"] <= 0) store.Remove("0");
            }
            else if (stone.Length % 2 == 1)
            {
                newStones.Add(Tuple.Create(Mul2024(stone), count));
                store[stone] -= count;
                if (store[stone] <= 0) store.Remove(stone);
            }
            else
            {
                int half = stone.Length / 2;
                string firstHalf = stone.Substring(0, half);
                string secondHalf = stone.Substring(half, half);
                newStones.Add(Tuple.Create(firstHalf, count));
                newStones.Add(Tuple.Create(secondHalf, count));
                store[stone] -= count;
                if (store[stone] <= 0) store.Remove(stone);
            }
        }

        newStones.ForEach(stone =>
        {
            string stoneKey = stone.Item1;
            long stoneValue = stone.Item2;

            stoneKey = long.Parse(stoneKey).ToString();
            store[stoneKey] = store.ContainsKey(stoneKey) ? store[stoneKey] + stoneValue : stoneValue;
        });

        blinks--;
    }

    long total = 0;
    foreach (var kvp in store)
    {
        total += kvp.Value;
    }
    return total;
}

string Mul2024(string s)
{
    return (long.Parse(s) * 2024).ToString();
}

void PrintStones(List<string> stones)
{
    if (!print) return;
    Console.WriteLine(string.Join(" ", stones));
}
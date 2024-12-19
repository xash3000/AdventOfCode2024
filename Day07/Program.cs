string inputFile = "input.txt";
// inputFile = "test.txt";

Dictionary<long, List<long>> equations = new();

foreach (string line in File.ReadLines(inputFile))
{
    string[] parts = line.Split(" ");
    parts[0] = parts[0].Replace(":", "");
    long key = long.Parse(parts[0]);
    List<long> values = new();
    for (int i = 1; i < parts.Length; i++)
    {
        values.Add(long.Parse(parts[i]));
    }
    values.Reverse();
    equations.Add(key, values);
}

long result = 0;

foreach (var equation in equations)
{
    if (Validate(equation.Key, equation.Value))
    {
        result += equation.Key;
    }
}

Console.WriteLine($"Part 1: {result}");

// Part 2

result = 0;

foreach (var equation in equations)
{
    if (Validate(equation.Key, equation.Value, true))
    {
        result += equation.Key;
    }
}

Console.WriteLine($"Part 2: {result}");

bool Validate(long target, List<long> values, bool concat = false)
{
    if (values.Count == 0) return false;
    if (values.Count == 1) return values[0] == target;
    if (values[^1] > target) return false;

    var addCopy = new List<long>(values);
    var sum = addCopy[^1] + addCopy[^2];
    addCopy.RemoveAt(addCopy.Count - 1);
    addCopy.RemoveAt(addCopy.Count - 1);
    addCopy.Add(sum);

    var mulCopy = new List<long>(values);
    var product = mulCopy[^1] * mulCopy[^2];
    mulCopy.RemoveAt(mulCopy.Count - 1);
    mulCopy.RemoveAt(mulCopy.Count - 1);
    mulCopy.Add(product);

    if (concat)
    {
        var concatCopy = new List<long>(values);
        var concatValue = long.Parse(concatCopy[^1].ToString() + concatCopy[^2].ToString());
        concatCopy.RemoveAt(concatCopy.Count - 1);
        concatCopy.RemoveAt(concatCopy.Count - 1);
        concatCopy.Add(concatValue);
        return Validate(target, addCopy, true) || Validate(target, mulCopy, true) || Validate(target, concatCopy, true);
    }
    return Validate(target, addCopy) || Validate(target, mulCopy);
}
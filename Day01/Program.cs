string inputFile = "input.txt";

var l1 = new List<int>();
var l2 = new List<int>();

foreach (string line in File.ReadLines(inputFile))
{
    int[] numbers = line.Split("   ").Select(int.Parse).ToArray();
    l1.Add(numbers[0]);
    l2.Add(numbers[1]);
}

// Part 1
l1.Sort();
l2.Sort();

int distance = 0;

for (int i = 0; i < l1.Count; i++)
{
    distance += Math.Abs(l1[i] - l2[i]);
}

Console.Write("Part 1: ");
Console.WriteLine(distance);

// Part 2
int similarity = 0;
foreach (int n in l1)
{
    similarity += n * l2.Count(x => x == n);
}

Console.Write("Part 2: ");
Console.WriteLine(similarity);
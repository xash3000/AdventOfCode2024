string inputFile = "input.txt";
// inputFile = "test.txt";

var lines = File.ReadAllLines(inputFile);

Dictionary<string, int> wires = new();

int lineIndex = 0;
for (lineIndex = 0; lineIndex < lines.Length; lineIndex++)
{
    if (string.IsNullOrEmpty(lines[lineIndex])) break;
    string[] line = lines[lineIndex].Split(": ");
    wires[line[0]] = int.Parse(line[1]);
}

Queue<string> instructions = new();

lineIndex++;
foreach (var ins in lines[lineIndex..])
{
    instructions.Enqueue(ins);
}
int zmax = 0;
while (instructions.Count > 0)
{
    string ins = instructions.Dequeue();
    string[] line = ins.Split(new char[] { ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries);
    string input1 = line[0];
    string op = line[1];
    string input2 = line[2];
    string output = line[3];

    if (!wires.ContainsKey(input1) || !wires.ContainsKey(input2))
    {
        instructions.Enqueue(ins);
        continue;
    }
    if (output.StartsWith('z'))
        zmax = Math.Max(zmax, int.Parse(output.Substring(1, output.Length - 1)));

    if (op == "AND") wires[output] = wires[input1] & wires[input2];
    if (op == "OR") wires[output] = wires[input1] | wires[input2];
    if (op == "XOR") wires[output] = wires[input1] ^ wires[input2];

}

int power = 0;
long result = 0;
for (int i = 0; i <= zmax; i++)
{
    result += (long)Math.Pow(2, power) * wires["z" + i.ToString("D2")];
    power++;
}

Console.WriteLine($"Part 1: {result}");
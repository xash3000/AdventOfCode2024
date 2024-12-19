
using System.Globalization;
using System.Text;

string inputFile = "input.txt";
// inputFile = "test2.txt";

string program = File.ReadAllText(inputFile);
int pos = 0;
int result = 0;

// Part 1
while (pos < program.Length)
{
    if (program[pos] == 'm')
    {
        ParseMul();
    }
    pos++;
}
Console.WriteLine($"Part 1: {result}");

// Part 2
pos = 0;
result = 0;

bool enabled = true;
while (pos < program.Length)
{
    if (program[pos] == 'm' && enabled)
    {
        ParseMul();
    }
    else if (program[pos] == 'd')
    {
        enabled = ParseConditional(enabled);
    }
    pos++;
}

Console.WriteLine($"Part 2: {result}");

void ParseMul(bool eval = true)
{
    pos++;
    if (!Expect('u')) return;
    pos++;
    if (!Expect('l')) return;
    pos++;
    if (!Expect('(')) return;
    pos++;

    var sb = new StringBuilder();
    while (!Expect(','))
    {
        if (!ExpectDigit()) return;
        sb.Append(program[pos]);
        pos++;
    }
    pos++;
    int n = int.Parse(sb.ToString());

    sb.Clear();
    while (!Expect(')'))
    {
        if (!ExpectDigit()) return;
        sb.Append(program[pos]);
        pos++;
    }
    int m = int.Parse(sb.ToString());

    if (!Expect(')')) return;

    if (eval) result += n * m;
}

bool Expect(char c)
{
    return program[pos] == c;
}

bool ExpectDigit()
{
    return char.IsDigit(program[pos]);
}

bool ParseConditional(bool enabled)
{
    pos++;
    if (!Expect('o')) return enabled;
    pos++;
    if (Expect('(')) // do statement
    {
        pos++;
        if (!Expect(')')) return enabled;
        return true;
    }
    else if (Expect('n')) // don't statement
    {
        pos++;
        if (!Expect('\'')) return enabled;
        pos++;
        if (!Expect('t')) return enabled;
        pos++;
        if (!Expect('(')) return enabled;
        pos++;
        if (!Expect(')')) return enabled;
        pos++;
        return false;
    }
    return enabled;
}


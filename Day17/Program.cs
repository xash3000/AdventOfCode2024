string inputFile = "input.txt";
// inputFile = "test.txt";

var lines = File.ReadAllLines(inputFile);

long A = long.Parse(lines[0].Split(" ")[^1]);
long B = long.Parse(lines[1].Split(" ")[^1]);
long C = long.Parse(lines[2].Split(" ")[^1]);

List<long> program = lines[4]
    .Replace("Program: ", "")
    .Split(",")
    .Select(long.Parse)
    .ToList();

List<long> output = Exec(program, A, B, C);
Console.WriteLine("Part 1: " + string.Join(",", output));

Console.WriteLine($"Part 2: {Search(0, program.Count - 1)}");

long Search(long A, long i)
{
    if (i < 0) return A >> 3;

    for (long k = A; k < A + 8; k++)
    {
        long val = Exec(program, k, 0, 0)[0];
        if (val == program[(int)i])
        {
            long temp = Search(k << 3, i - 1);
            if (temp != -1) return temp;
        }
    }
    return -1;
}

List<long> Exec(List<long> program, long A, long B, long C)
{
    long ip = 0;

    List<long> output = new();

    while (ip < program.Count)
    {
        long instruction = program[(int)ip];
        long op = program[(int)ip + 1];
        long comnoOp = Combo(op, A, B, C);

        bool jump = true;
        switch (instruction)
        {
            case 0:
                A = Xdv(A, comnoOp);
                break;
            case 1:
                B = Bxl(B, op);
                break;
            case 2:
                B = Bst(comnoOp);
                break;
            case 3:
                ip = Jnz(A, op, ip);
                if (A != 0) jump = false;
                break;
            case 4:
                B = Bxc(B, C, op);
                break;
            case 5:
                output.Add(Out(comnoOp));
                break;
            case 6:
                B = Xdv(A, comnoOp);
                break;
            case 7:
                C = Xdv(A, comnoOp);
                break;
        }

        if (jump)
        {
            ip += 2;
        }
    }
    return output;
}

long Combo(long op, long A, long B, long C)
{
    if (op == 4) return A;
    if (op == 5) return B;
    if (op == 6) return C;

    return op;
}

long Xdv(long A, long op)
{
    return (long)(A / Math.Pow(2, op));
}

long Bxl(long B, long op)
{
    return B ^ op;
}

long Bst(long op)
{
    return op % 8;
}

long Jnz(long A, long op, long ip)
{
    if (A == 0) return ip;
    return op;
}

long Bxc(long B, long C, long op)
{
    return B ^ C;
}

long Out(long op)
{
    return op % 8;
}
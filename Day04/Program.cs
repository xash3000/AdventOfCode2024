using System.Text;

string inputFile = "input.txt";
// inputFile = "test.txt";

var lines = File.ReadAllLines(inputFile);

// Part 1
int count = 0;
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[0].Length; j++)
    {
        if (lines[i][j] == 'X')
        {
            count += Search(i, j, 0, 1);   // Right
            count += Search(i, j, 1, 0);   // Down
            count += Search(i, j, 1, 1);   // Diagonal
            count += Search(i, j, 1, -1);  // Diagonal 2
            count += Search(i, j, 0, -1);  // Left
            count += Search(i, j, -1, 0);  // Up
            count += Search(i, j, -1, 1);  // Reverse Diagonal
            count += Search(i, j, -1, -1); // Reverse Diagonal 2
        }
    }
}

Console.WriteLine($"Part 1: {count}");

// Part 2
count = 0;
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[0].Length; j++)
    {
        if (lines[i][j] == 'M' || lines[i][j] == 'S')
        {
            count += SearchXMAS(i, j);
        }
    }
}

Console.WriteLine($"Part 2: {count}");

int Search(int i, int j, int deltaI, int deltaJ)
{
    var sb = new StringBuilder();
    for (int k = 0; k < 4; k++)
    {
        sb.Append(Get(i + k * deltaI, j + k * deltaJ));
    }
    return sb.ToString() == "XMAS" ? 1 : 0;
}

int SearchXMAS(int i, int j)
{
    var sb = new StringBuilder();
    for (int k = 0; k < 3; k++)
    {
        sb.Append(Get(i + k, j + k));
    }
    if (sb.ToString() != "MAS" && sb.ToString() != "SAM") return 0;

    sb.Clear();
    j += 2; // Move to the end of the diagonal
    for (int k = 0; k < 3; k++)
    {
        sb.Append(Get(i + k, j - k));
    }
    return sb.ToString() == "SAM" || sb.ToString() == "MAS" ? 1 : 0;
}

char Get(int i, int j)
{
    try
    {
        return lines[i][j];
    }
    catch
    {
        return ' ';
    }
}

string inputFile = "input.txt";
var reports = new List<List<int>>();
foreach (var line in File.ReadLines(inputFile))
{
    reports.Add(line.Split(' ').Select(int.Parse).ToList<int>());
}

// Part 1
int safe = 0;
foreach (var report in reports)
{
    if (IsReportSafe(report)) safe++;
}
Console.WriteLine($"Part 1: {safe}");

// Part 2
int almostSafe = 0;
foreach (var report in reports)
{
    if (IsReportAlmostSafe(report)) almostSafe++;
}
Console.WriteLine($"Part 2: {almostSafe}");

bool IsReportSafe(List<int> report)
{
    bool isIncreasing = true, isDecreasing = true;
    for (int i = 1; i < report.Count; i++)
    {
        int difference = report[i] - report[i - 1];
        if (difference < 1 || difference > 3) isIncreasing = false;
        if (difference > -1 || difference < -3) isDecreasing = false;
        if (!isIncreasing && !isDecreasing) return false;
    }
    return isIncreasing || isDecreasing;
}

bool IsReportAlmostSafe(List<int> report)
{
    if (IsReportSafe(report)) return true;
    for (int i = 0; i < report.Count; i++)
    {
        var copy = new List<int>(report);
        copy.RemoveAt(i);
        if (IsReportSafe(copy)) return true;
    }
    return false;
}

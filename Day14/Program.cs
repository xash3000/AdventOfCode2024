using SkiaSharp;

string inputFile = "input.txt";
// inputFile = "test.txt";

var robots = File.ReadAllLines(inputFile)
    .Where(line => !string.IsNullOrWhiteSpace(line))
    .Select(line =>
    {
        var nums = line.Split(new[] { 'p', '=', ',', 'v', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return new Robot(
            int.Parse(nums[0]),
            int.Parse(nums[1]),
            int.Parse(nums[2]),
            int.Parse(nums[3])
        );
    })
    .ToArray();

var part2Robots = robots.Select(r => new Robot(r.x, r.y, r.vx, r.vy)).ToArray();

int width = 101, height = 103, steps = 100;
if (inputFile == "test.txt")
{
    width = 7;
    height = 11;
}
int q1 = 0, q2 = 0, q3 = 0, q4 = 0;

foreach (var robot in robots)
{
    robot.Move(steps, width, height);

    if (robot.x == width / 2 || robot.y == height / 2) continue;
    if (robot.x < width / 2 && robot.y < height / 2) q1++;
    else if (robot.x >= width / 2 && robot.y < height / 2) q2++;
    else if (robot.x < width / 2 && robot.y >= height / 2) q4++;
    else if (robot.x >= width / 2 && robot.y >= height / 2) q3++;
}

Console.WriteLine($"Q1: {q1}, Q2: {q2}, Q3: {q3}, Q4: {q4}");
Console.WriteLine($"Part 1: Safety Factor: {q1 * q2 * q3 * q4}");

// Part 2
string outputFolder = "Steps";
Directory.CreateDirectory(outputFolder);

int maxSteps = 10000;

for (int i = 0; i < maxSteps; i++)
{
    try
    {
        Console.WriteLine($"Generating Step {i + 1}");
        foreach (var robot in part2Robots)
        {
            robot.Move(1, width, height);
        }

        using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
        {
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.Black;
                paint.StrokeWidth = 3;
                paint.Style = SKPaintStyle.Stroke;

                foreach (var robot in part2Robots)
                {
                    canvas.DrawPoint(robot.x, robot.y, paint);
                }
            }

            using (var image = surface.Snapshot())
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                if (data == null)
                {
                    Console.WriteLine($"Failed to encode image for Step {i + 1}");
                    continue;
                }

                string filePath = Path.Combine(outputFolder, $"Step_{i + 1}.png");

                File.WriteAllBytes(filePath, data.ToArray());

                Console.WriteLine($"Step {i + 1} saved successfully.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error generating Step {i + 1}: {ex.Message}");
    }
}



class Robot
{
    public int x, y, vx, vy;

    public Robot(int x, int y, int vx, int vy)
    {
        this.x = x;
        this.y = y;
        this.vx = vx;
        this.vy = vy;
    }

    public void Move(int steps, int width, int height)
    {
        x = (x + vx * steps) % width;
        if (x < 0) x += width;
        y = (y + vy * steps) % height;
        if (y < 0) y += height;
    }
}


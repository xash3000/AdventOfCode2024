using System.Numerics;

class Part1
{
    private string inputFile = "input.txt";
    private List<List<char>> grid = new List<List<char>>();
    private Part1Node startNode = new Part1Node(Vector2.Zero, Direction.Right, null, 0);
    private Part1Node endNode = new Part1Node(Vector2.Zero, Direction.Up, null, 0);
    private int width, height;
    private PriorityQueue<Part1Node, float> openList = new PriorityQueue<Part1Node, float>();
    private List<Part1Node> closedList = new List<Part1Node>();

    public void Run()
    {
        ReadInput();
        SolvePart1();
    }

    private void ReadInput()
    {
        var lines = File.ReadAllLines(inputFile);
        int i = 0;

        foreach (var line in lines)
        {
            if (line.Contains("S"))
            {
                var pos = new Vector2(line.IndexOf("S"), i);
                startNode = new Part1Node(pos, Direction.Right, null, 0);
            }
            else if (line.Contains("E"))
            {
                var pos = new Vector2(line.IndexOf("E"), i);
                endNode = new Part1Node(pos, Direction.Up, null, 0);
            }
            grid.Add(line.ToList());
            i++;
        }

        width = grid[0].Count;
        height = grid.Count;
    }

    private void SolvePart1()
    {
        List<Part1Node> bestPaths = new List<Part1Node>();
        startNode.DistanceToTarget = (int)(Math.Abs(startNode.Position.X - endNode.Position.X) + Math.Abs(startNode.Position.Y - endNode.Position.Y));
        startNode.F = startNode.DistanceToTarget;
        openList.Enqueue(startNode, startNode.F);

        while (openList.Count > 0)
        {
            var currentNode = openList.Dequeue();

            if (currentNode.Position == endNode.Position)
            {
                bestPaths.Add(currentNode);
                continue;
            }

            var adjNodes = GetAdjNodes(currentNode);
            foreach (var adjNode in adjNodes)
            {
                if (PathVisited(adjNode) || FoundBetterPath(adjNode))
                {
                    continue;
                }
                openList.Enqueue(adjNode, adjNode.F);
            }
            closedList.Add(currentNode);
        }

        Console.WriteLine($"Part 1: {bestPaths[0].Cost}");
    }

    private bool PathVisited(Part1Node adjNode)
    {
        return closedList.Any(n => n.Position == adjNode.Position && n.F < adjNode.F);
    }

    private bool FoundBetterPath(Part1Node adjNode)
    {
        foreach ((Part1Node node, float priority) in openList.UnorderedItems)
        {
            if (node.Position == adjNode.Position && node.F < adjNode.F)
            {
                return true;
            }
        }
        return false;
    }

    private bool Valid(Vector2 pos)
    {
        return pos.X >= 0 && pos.X < width && pos.Y >= 0 && pos.Y < height;
    }

    private List<Part1Node> GetAdjNodes(Part1Node node)
    {
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };
        List<Part1Node> adjNodes = new List<Part1Node>();

        for (int i = 0; i < 4; i++)
        {
            Vector2 newPos = new Vector2(node.Position.X + dx[i], node.Position.Y + dy[i]);
            if (Valid(newPos) && grid[(int)newPos.Y][(int)newPos.X] != '#')
            {
                int distanceToTarget = (int)(Math.Abs(newPos.X - endNode.Position.X) + Math.Abs(newPos.Y - endNode.Position.Y));
                adjNodes.Add(new Part1Node(newPos, (Direction)i, node, distanceToTarget));
            }
        }
        return adjNodes;
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}

class Part1Node
{
    public Vector2 Position { get; private set; }
    public Direction Direction { get; private set; }
    public Part1Node? Parent { get; private set; }
    public int Cost { get; private set; }
    private int Weight { get; set; }
    public int DistanceToTarget { get; set; }
    public int F { get; set; }

    public Part1Node(Vector2 position, Direction direction, Part1Node? parent, int distanceToTarget)
    {
        Position = position;
        Direction = direction;
        Parent = parent;
        DistanceToTarget = distanceToTarget;
        CalculateCost();
    }

    private void CalculateCost()
    {
        if (Parent == null)
        {
            Cost = 0;
            F = Cost + DistanceToTarget;
            return;
        }

        Weight = 1;
        if (Parent.Direction != Direction)
        {
            Weight += 1000;
            if (Parent.Direction == Direction.Up && Direction == Direction.Down) Weight += 1000;
            if (Parent.Direction == Direction.Down && Direction == Direction.Up) Weight += 1000;
            if (Parent.Direction == Direction.Left && Direction == Direction.Right) Weight += 1000;
            if (Parent.Direction == Direction.Right && Direction == Direction.Left) Weight += 1000;
        }

        Cost = Parent.Cost + Weight;
        F = Cost + DistanceToTarget;
    }
}

string inputFile = "input.txt";
// inputFile = "test.txt";

Dictionary<string, HashSet<string>> graph = new();
Dictionary<string, int> degree = new();
HashSet<string> nodesSet = new();

foreach (string line in File.ReadAllLines(inputFile))
{
    string[] computers = line.Split('-');

    if (!graph.ContainsKey(computers[0]))
    {
        graph[computers[0]] = new HashSet<string>();
        degree[computers[0]] = 0;
    }
    if (!graph.ContainsKey(computers[1]))
    {
        degree[computers[1]] = 0;
        graph[computers[1]] = new HashSet<string>();
    }

    graph[computers[0]].Add(computers[1]);
    degree[computers[0]]++;
    graph[computers[1]].Add(computers[0]);
    degree[computers[1]]++;

    nodesSet.Add(computers[0]);
    nodesSet.Add(computers[1]);
}

HashSet<HashSet<string>> cliques = new();
List<string> nodes = nodesSet.ToList();

FindCliques(0, new HashSet<string>(), 3);

int result = 0;
foreach (var clique in cliques)
{
    if (clique.Any(node => node.StartsWith('t'))) result++;
}

Console.WriteLine($"Part 1: {result}");

HashSet<string> maxClique = new();

MaximalClique(0, new HashSet<string>());

Console.WriteLine("Part2: " + string.Join(',', maxClique.ToList().OrderBy(e => e)));

void FindCliques(int startIndex, HashSet<string> currentClique, int targetSize)
{
    if (currentClique.Count == targetSize)
    {
        cliques.Add(new HashSet<string>(currentClique));
        return;
    }

    for (int i = startIndex; i < nodes.Count; i++)
    {
        string candidateNode = nodes[i];
        if (degree[candidateNode] < targetSize - 1) continue;

        currentClique.Add(candidateNode);

        if (IsClique(currentClique))
        {
            FindCliques(i + 1, currentClique, targetSize);
        }
        currentClique.Remove(candidateNode);
    }
}

bool IsClique(HashSet<string> currentClique)
{
    List<string> nodes = currentClique.ToList();

    for (int i = 0; i < nodes.Count; i++)
    {
        for (int j = 0; j < nodes.Count; j++)
        {
            if (i == j) continue;
            if (!graph[nodes[i]].Contains(nodes[j])) return false;
            if (!graph[nodes[j]].Contains(nodes[i])) return false;
        }
    }
    return true;
}

void MaximalClique(int startIndex, HashSet<string> currentClique)
{
    if (currentClique.Count >= maxClique.Count) maxClique = new HashSet<string>(currentClique);
    if (startIndex >= nodes.Count) return;

    for (int i = startIndex; i < nodes.Count; i++)
    {
        var candidateNode = nodes[i];

        currentClique.Add(candidateNode);
        if (IsClique(currentClique))
        {
            MaximalClique(i + 1, currentClique);
        }
        currentClique.Remove(candidateNode);
    }
}
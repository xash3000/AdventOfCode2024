string inputFile = "input.txt";
// inputFile = "test.txt";

string[] lines = File.ReadAllLines(inputFile);

var rules = new Dictionary<int, List<int>>();
var pages = new List<List<int>>();

int i;
for (i = 0; i < lines.Length; i++)
{
    if (string.IsNullOrWhiteSpace(lines[i])) break;
    int[] order = lines[i].Split('|').Select(int.Parse).ToArray();

    if (!rules.ContainsKey(order[0])) rules[order[0]] = new List<int>();
    if (!rules.ContainsKey(order[1])) rules[order[1]] = new List<int>();
    rules[order[0]].Add(order[1]);
}

for(int k = i + 1; k < lines.Length; k++){
    pages.Add(lines[k].Split(',').Select(int.Parse).ToList());
}

int result = 0;
foreach (var updates in pages)
{
    if(ValidateOrder(updates)){
        result += updates[(int)updates.Count / 2];
    }
}

Console.WriteLine($"Part 1: {result}");

result = 0;
foreach(var updates in pages){
    if(!ValidateOrder(updates)){
        updates.Sort((a, b) => {
            if(a == b) return 0;
            if(rules[a].Contains(b)) return -1;
            return 1;
        });
        result += updates[(int)updates.Count / 2];
    }
}

Console.WriteLine($"Part 2: {result}");

bool ValidateOrder(List<int> updates){
    for(int i=0; i < updates.Count; i++){
        for(int j=i + 1; j < updates.Count; j++){
            if(rules[updates[j]].Contains(updates[i])) return false;
        }
    }
    return true;
}
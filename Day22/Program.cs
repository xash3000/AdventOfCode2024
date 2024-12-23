string inputFile = "input.txt";
// inputFile = "test.txt";

int iters = 2000;

long[] secrets = File.ReadLines(inputFile).Select(long.Parse).ToArray();
Dictionary<(long, long, long, long), long> seqs = new();

for (int i = 0; i < secrets.Length; i++)
{
    long s1 = 0, s2 = 0, s3 = 0, s4 = 0;
    HashSet<(long, long, long, long)> firstApperance = new();
    for (int j = 0; j < iters; j++)
    {
        long prevPrice = secrets[i] % 10;
        secrets[i] = PRNG(secrets[i]);
        long newPrice = secrets[i] % 10;

        s1 = s2;
        s2 = s3;
        s3 = s4;
        s4 = newPrice - prevPrice;

        if (j >= 3 && !firstApperance.Contains((s1, s2, s3, s4)))
        {
            if (!seqs.ContainsKey((s1, s2, s3, s4)))
                seqs[(s1, s2, s3, s4)] = 0;
            seqs[(s1, s2, s3, s4)] += newPrice;
            firstApperance.Add((s1, s2, s3, s4));
        }
    }
}

Console.WriteLine($"Part 1: {secrets.Sum()}");

Console.WriteLine($"Part 2: {seqs.Max(kvp => kvp.Value)}");

long PRNG(long secret)
{
    secret = ((secret * 64L) ^ secret) % 16777216L;
    long div = (long)Math.Floor((double)secret / 32);
    secret = secret ^ div;
    secret = secret % 16777216L;
    secret = ((secret * 2048L) ^ secret) % 16777216L;
    return secret;
}
string inputFile = "input.txt";
// inputFile = "test.txt";

long[] secrets = File.ReadLines(inputFile).Select(long.Parse).ToArray();

int iters = 2000;

for (int i = 0; i < iters; i++)
{
    for (int j = 0; j < secrets.Length; j++)
    {
        secrets[j] = PRNG(secrets[j]);
    }
}

Console.WriteLine($"Part 1: {secrets.Sum()}");

long PRNG(long secret)
{
    secret = ((secret * 64L) ^ secret) % 16777216L;
    long div = (long)Math.Floor((double)secret / 32);
    secret = secret ^ div;
    secret = secret % 16777216L;
    secret = ((secret * 2048L) ^ secret) % 16777216L;
    return secret;
}
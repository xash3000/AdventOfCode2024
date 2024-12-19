string inputFile = "input.txt";
// inputFile = "test.txt";

int[] diskMap = File.ReadAllText(inputFile)
                    .ToCharArray().
                    Select(c => int.Parse(c.ToString()))
                    .ToArray();

var disk = new List<string>();
int fileID = 0;

for (int i = 0; i < diskMap.Length; i++)
{
    if (i % 2 == 0) // file block
    {
        for (int j = 0; j < diskMap[i]; j++)
        {
            disk.Add(fileID.ToString());
        }
        fileID++;
    }
    else // free block
    {
        for (int j = 0; j < diskMap[i]; j++) disk.Add(".");
    }
}

var diskCopy = disk.ToList();

int spacePointer = 0;
int filePointer = disk.Count - 1;

while (spacePointer < filePointer)
{
    while (disk[spacePointer] != ".") spacePointer++;
    while (disk[filePointer] == ".") filePointer--;
    if (spacePointer >= filePointer) break;

    string temp = disk[spacePointer];
    disk[spacePointer] = disk[filePointer];
    disk[filePointer] = temp;
}

long checksum = 0;
for (long pos = 0; pos < disk.Count; pos++)
{
    string id = disk[(int)pos];
    if (id == ".") continue;
    checksum += long.Parse(id.ToString()) * pos;
}

Console.WriteLine($"Part 1: {checksum}");

// Part 2
disk = diskCopy;

spacePointer = 0;
filePointer = disk.Count - 1;

int spaceSize = 0;
int spaceSizePointer = 0;

int fileSize = 0;
int fileSizePointer = 0;

WriteDisk();

while (spacePointer < filePointer)
{
    while (filePointer > 0 && disk[filePointer] == ".") filePointer--;
    if (filePointer <= 0) break;
    fileSizePointer = filePointer;
    fileSize = 0;
    while (disk[fileSizePointer] == disk[filePointer])
    {
        if (fileSizePointer <= 0) break;
        fileSizePointer--;
        fileSize++;
    }


    while (spacePointer < filePointer)
    {
        while (spacePointer < filePointer && disk[spacePointer] != ".") spacePointer++;
        if (spacePointer >= filePointer) break;
        spaceSizePointer = spacePointer;
        spaceSize = 0;
        while (disk[spaceSizePointer] == ".")
        {
            spaceSizePointer++;
            spaceSize++;
        }
        if (spaceSize < fileSize)
        {
            spacePointer += spaceSize;
            continue;
        }
        else
        {
            break;
        }
    }

    if (spacePointer >= filePointer)
    {
        spacePointer = 0;
        filePointer -= fileSize;
        continue;
    }

    int spaceStart = spacePointer;
    int nextFilePointer = filePointer - 1;

    while (fileSize > 0)
    {
        string temp = disk[spacePointer];
        disk[spacePointer] = disk[filePointer];
        disk[filePointer] = temp;

        spacePointer++;
        filePointer--;
        fileSize--;
    }
    spacePointer = 0;
    filePointer = nextFilePointer;
    WriteDisk();
}

WriteDisk();

checksum = 0;
for (long pos = 0; pos < disk.Count; pos++)
{
    string id = disk[(int)pos];
    if (id == ".") continue;
    checksum += long.Parse(id.ToString()) * pos;
}

Console.WriteLine($"Part 2: {checksum}");

void WriteDisk()
{
    if (inputFile != "test.txt") return;

    foreach (var item in disk)
    {
        Console.Write($"{item}|");
    }
    Console.WriteLine();
}
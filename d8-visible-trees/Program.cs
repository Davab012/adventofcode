using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

part2("in.txt");

static void part1(string file)
{
    // Only the previous and next lines are needed
    var fileLines = File.ReadAllLines(file);
    
    var width = fileLines[0].Length;

    // Create an int array with only -1
    var emptyLine = Enumerable.Repeat(-1, width+2).ToArray();

    var list = new List<int[]>();
    list.Add(emptyLine);
    foreach(var l in fileLines)
    {
        list.Add(LineToIntArray(l));
    }
    list.Add(emptyLine);

    var lines = list.ToArray();

    var total = 0;
    var prevLine = lines[0];
    var currLine = lines[1];    
    for (var i = 1; i < lines.Length-1; i++)
    {
        // For each position in the currLine, look left, right, up and below
        for (var j = 1; j < currLine.Length - 1; j++)
        {
            if (IsVisible(lines, i, j, -1, 0)
                || IsVisible(lines, i, j, 1, 0)
                || IsVisible(lines, i, j, 0, -1)
                || IsVisible(lines, i, j, 0, 1))
            {
                Console.WriteLine($"Tree {lines[i][j]} at row {i} col {j} is visible");
                total++;
            }
        }
    }
    Console.WriteLine(total);
}
static bool IsVisible(int[][] lines, int currY, int currX, int moveX, int moveY)
{
    var thisTree = lines[currY][currX];
    
    do
    {
        currX += moveX;
        currY += moveY;
        if (lines[currY][currX] >= thisTree)
            return false;
        if (lines[currY][currX] == -1)
        {
            return true;
        }
    } while (true);
}

static void part2(string file)
{
    // Only the previous and next lines are needed
    var fileLines = File.ReadAllLines(file);

    var width = fileLines[0].Length;

    // Create an int array with only -1
    var emptyLine = Enumerable.Repeat(-1, width + 2).ToArray();

    var list = new List<int[]>();
    list.Add(emptyLine);
    foreach (var l in fileLines)
    {
        list.Add(LineToIntArray(l));
    }
    list.Add(emptyLine);

    var lines = list.ToArray();

    var maxScenic = 0;
    var prevLine = lines[0];
    var currLine = lines[1];
    
    for (var i = 1; i < lines.Length - 1; i++)
    {
        // For each position in the currLine, look left, right, up and below
        for (var j = 1; j < currLine.Length - 1; j++)
        {
            var scenicScore = NumberOfVisibleTrees(lines, i, j, -1, 0)
                * NumberOfVisibleTrees(lines, i, j, 1, 0)
                * NumberOfVisibleTrees(lines, i, j, 0, -1)
                * NumberOfVisibleTrees(lines, i, j, 0, 1);
            maxScenic = scenicScore > maxScenic ? scenicScore : maxScenic;
        }
    }
    Console.WriteLine($"Max scenic score: {maxScenic}");
}

static int NumberOfVisibleTrees(int[][] lines, int currY, int currX, int moveX, int moveY)
{
    var thisTree = lines[currY][currX];
    var cnt = 0;
    do
    {
        currX += moveX;
        currY += moveY;
        if (lines[currY][currX] >= thisTree)
            return cnt+1;
        if (lines[currY][currX] == -1)
        {
            return cnt;
        }
        cnt++;
    } while (true);
}


static int[] LineToIntArray(string line)
{
    var arr = new int[line.Length+2];
    for (var i = 0; i < line.Length; i++)
    {
        arr[i+1] = int.Parse(line.Substring(i,1));
    }
    arr[0] = -1;
    arr[line.Length + 1] = -1;
    return arr;
}
using d14;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

// To run part 1 with full data and display the simulation the console needs to be set to a lower font size, like 3
part2("in.txt");

static void part1(string file)
{
    var fileLines = File.ReadAllLines(file);

    int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
    
    // Create a list of lists of strings
    var linesToDraw = new List<List<(int, int)>>();
    
    // Get map size (min max)
    foreach (var line in fileLines)
    {
        var currLineToDraw = new List<(int, int)>();
        foreach(Match match in Regex.Matches(line, "(\\d+),(\\d+)"))
        {
            var x = int.Parse(match.Groups[1].Value);
            var y = int.Parse(match.Groups[2].Value);
            currLineToDraw.Add((x, y));
            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
            if (y < minY) minY = y;
            if (y > maxY) maxY = y;
        }
        linesToDraw.Add(currLineToDraw);
    }

    var map = new Map(maxX - minX + 1, maxY + 1) { MinX = minX};
    foreach (var lineToDraw in linesToDraw)
    {
        var lastLine = lineToDraw[0];
        for (var i = 1; i < lineToDraw.Count; i++) 
        {
            var subLine = lineToDraw[i];
            map.DrawLine(lastLine.Item1, lastLine.Item2, subLine.Item1, subLine.Item2, '#');
            lastLine = subLine;
        }
    }
    map.Display();
    
    var sim = new Simulator { Map = map };
    int total = 0;
    do
    {
        sim.SandX = 500;
        sim.SandY = 0;
        total++;
    } while (sim.Run(0));

    // Print sands at rest
    Console.WriteLine($"Sands at rest: {total-1}");
}

static void part2(string file)
{
    var fileLines = File.ReadAllLines(file);

    int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;

    // Create a list of lists of strings
    var linesToDraw = new List<List<(int, int)>>();

    // Get map size (min max)
    foreach (var line in fileLines)
    {
        var currLineToDraw = new List<(int, int)>();
        foreach (Match match in Regex.Matches(line, "(\\d+),(\\d+)"))
        {
            var x = int.Parse(match.Groups[1].Value);
            var y = int.Parse(match.Groups[2].Value);
            currLineToDraw.Add((x, y));
            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
            if (y < minY) minY = y;
            if (y > maxY) maxY = y;
        }
        linesToDraw.Add(currLineToDraw);
    }

    // Add the "infinite" floor, which is actually limited by the max Y
    maxY += 2;
    minX = 500 - maxY - 1;
    maxX = 500 + maxY + 1;
    linesToDraw.Add(new List<(int, int)>() { (minX, maxY), (maxX, maxY) });

    var map = new Map(maxX - minX + 1, maxY + 1) { MinX = minX };
    foreach (var lineToDraw in linesToDraw)
    {
        var lastLine = lineToDraw[0];
        for (var i = 1; i < lineToDraw.Count; i++)
        {
            var subLine = lineToDraw[i];
            map.DrawLine(lastLine.Item1, lastLine.Item2, subLine.Item1, subLine.Item2, '#');
            lastLine = subLine;
        }
    }
    //map.Display();

    var sim = new Simulator { Map = map };
    int total = 0;
    do
    {
        sim.SandX = 500;
        sim.SandY = 0;
        total++;
    } while (sim.Run(0, false) && map.GetTile(500, 0) == '.');

    // Print sands at rest
    Console.WriteLine($"Sands at rest: {total}");
}


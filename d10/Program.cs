// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

//part1("in.txt");
part2("in.txt");

void part1(string file)
{
    var allLines = File.ReadAllLines(file);
    var registerAtCycle = new List<int>();
    
    var register = 1;
    // Just to start 1 based...
    registerAtCycle.Add(register);
    for (var currLine = 0; currLine < allLines.Length; currLine++)
    {
        registerAtCycle.Add(register);
        var line = allLines[currLine];

        if (line.StartsWith("addx"))
        {
            var newValue = int.Parse(line.Substring(5));
            // takes one extra cycle to complete, keep the current register and update it
            // and the next cycle.
            registerAtCycle.Add(register);
            register += newValue;
        }
    }

    var total = 0;
    for (var i = 20; i < registerAtCycle.Count; i+=40)
    {
        var toAdd = i * registerAtCycle[i];
        total += toAdd;
        Console.WriteLine($"cycle: {i} to add: {toAdd} total: {total}");
    }
}

void part2(string file)
{
    var allLines = File.ReadAllLines(file);
    var registerAtCycle = new List<int>();

    var register = 1;
    // Just to start 1 based...
    registerAtCycle.Add(register);
    for (var currLine = 0; currLine < allLines.Length; currLine++)
    {
        registerAtCycle.Add(register);
        var line = allLines[currLine];

        if (line.StartsWith("addx"))
        {
            var newValue = int.Parse(line.Substring(5));
            // takes one extra cycle to complete, keep the current register and update it
            // and the next cycle.
            registerAtCycle.Add(register);
            register += newValue;
        }
    }

    for (var cycle = 1; cycle < registerAtCycle.Count; cycle++)
    {
        var drawingAtX = (cycle - 1) % 40;
        if (drawingAtX == 0)
        {
            Console.WriteLine();
        }

        register = registerAtCycle[cycle];
        if (drawingAtX >= register - 1 && drawingAtX <= register + 1)
        {
            Console.Write("X");
        }
        else
        {
            Console.Write(".");
        }
    }
}
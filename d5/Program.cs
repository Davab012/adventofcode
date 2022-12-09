using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

part2("in.txt", 9);

static void part1(string file, int stackCount)
{
    var stacks = new Stack<string>[stackCount];
    for (var i = 0; i < stackCount; i++)
    {
        stacks[i] = new Stack<string>();
    }

    bool isInitializing = true;
    foreach (var line in File.ReadLines(file))
    {
        if (isInitializing && line[1] == '1')
        {
            isInitializing = false;
            // Reverse the stacks
            for (var i = 0; i < stackCount; i++)
            {
                stacks[i] = new Stack<string>(stacks[i]);
            }
            continue;
        }

        if (isInitializing)
        { 
            for (var i = 0; i < stackCount; i++)
            {
                var letter = line.Substring(i * 4 + 1, 1);
                if (letter != " ")
                {
                    stacks[i].Push(letter);
                }
            }
        }
        else
        {
            // Parse command
            // example: move 1 from 2 to 1
            Console.WriteLine(line);
            var m = Regex.Match(line, "move (?<num>(\\d+)) from (?<src>(\\d+)) to (?<tgt>(\\d+))");
            if (m.Success)
            {
                var f = int.Parse(m.Groups["src"].Value) - 1;
                var t = int.Parse(m.Groups["tgt"].Value) - 1;
                var cnt = int.Parse(m.Groups["num"].Value);
                for (var j = 0; j < cnt; j++)
                {
                    var curr = stacks[f].Pop();
                    Console.WriteLine($"    Moving {curr} from {f} to {t}");
                    stacks[t].Push(curr);
                }
            }
        }
    }

    for (var i = 0; i < stackCount; i++)
    {
        Console.Write(stacks[i].Peek());
    }
}

static void part2(string file, int stackCount)
{
    var stacks = new Stack<string>[stackCount];
    for (var i = 0; i < stackCount; i++)
    {
        stacks[i] = new Stack<string>();
    }

    bool isInitializing = true;
    foreach (var line in File.ReadLines(file))
    {
        if (isInitializing && line[1] == '1')
        {
            isInitializing = false;
            // Reverse the stacks
            for (var i = 0; i < stackCount; i++)
            {
                stacks[i] = new Stack<string>(stacks[i]);
            }
            continue;
        }

        if (isInitializing)
        {
            for (var i = 0; i < stackCount; i++)
            {
                var letter = line.Substring(i * 4 + 1, 1);
                if (letter != " ")
                {
                    stacks[i].Push(letter);
                }
            }
        }
        else
        {
            // Parse command
            // example: move 1 from 2 to 1
            Console.WriteLine(line);
            var m = Regex.Match(line, "move (?<num>(\\d+)) from (?<src>(\\d+)) to (?<tgt>(\\d+))");
            if (m.Success)
            {
                var f = int.Parse(m.Groups["src"].Value) - 1;
                var t = int.Parse(m.Groups["tgt"].Value) - 1;
                var cnt = int.Parse(m.Groups["num"].Value);
                var tempStack = new Stack<string>();
                for (var j = 0; j < cnt; j++)
                {
                    var curr = stacks[f].Pop();
                    tempStack.Push(curr);
                }
                foreach(string curr in tempStack)
                {
                    Console.WriteLine($"    Moving {curr} from {f} to {t}");
                    stacks[t].Push(curr);
                }
            }
        }
    }

    for (var i = 0; i < stackCount; i++)
    {
        Console.Write(stacks[i].Peek());
    }
}

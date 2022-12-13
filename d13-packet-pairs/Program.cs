using System.Collections.Generic;
using System.Net.Sockets;
using d13_packet_pairs;

part2("in.txt");

static void part1(string file)
{
    var fileLines = File.ReadAllLines(file);

    var currLine = 0;
    int pairIndex = 0;
    int total = 0;
    while (currLine < fileLines.Length)
    {
        //Console.Clear();
        pairIndex++;
        Util.Log("");
        Util.Log($"== Pair {pairIndex} ==");

        // Always work with pairs
        var left = fileLines[currLine];
        currLine++;
        var right = fileLines[currLine];
        currLine += 2;

        // Or we can get the first element from the left and the first element from the right
        if (Util.Compare(left, right) < 0)
        {
            Util.Log($"Pair {pairIndex} was in order");
            total += pairIndex;
            // Correct order
        }else
        {
            Util.Log($"Pair {pairIndex} was -NOT- in order");
        }
    }

    Console.WriteLine($"Total: {total}");
}

// Order all packets, no longer care about pairs.
static void part2(string file)
{
    var fileLines = File.ReadAllLines(file);

    var currLine = 0;
    var packets = new List<string>();
    while (currLine < fileLines.Length)
    {
        packets.Add(fileLines[currLine]);
        currLine++;
        packets.Add(fileLines[currLine]);
        currLine += 2;
    }

    // Before sorting

    // Add the two extra packets
    var div1 = "[[2]]";
    var div2 = "[[6]]";
    packets.Add(div1);
    packets.Add(div2);

    // Display before sorting
    Console.WriteLine("== BEFORE ==");
    Console.WriteLine(string.Join("\n", packets));

    // Sort everything
    packets.Sort((a, b) => Util.Compare(a, b));

    // And display
    Console.WriteLine("== AFTER ==");
    Console.WriteLine(string.Join("\n", packets));

    Console.WriteLine("Answer: " + (packets.IndexOf(div1) + 1) * (packets.IndexOf(div2) + 1));
}
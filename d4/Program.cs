using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

part2();

static void part1()
{
    var total = 0;
    foreach (var line in File.ReadLines("in.txt"))
    {
        var match = Regex.Match(line, "(?<min1>\\d+)-(?<max1>\\d+),(?<min2>\\d+)-(?<max2>\\d+)");
        var d = new { 
            min1 = int.Parse(match.Groups["min1"].Value),
            max1 = int.Parse(match.Groups["max1"].Value),
            min2 = int.Parse(match.Groups["min2"].Value),
            max2 = int.Parse(match.Groups["max2"].Value),
        };
        var pt = 1;
        if (d.min1 <= d.min2 && d.max1 >= d.max2)
        {
            Console.WriteLine($"{d.min2}-{d.max2} contained in {d.min1}-{d.max1}");
        }else if (d.min2 <= d.min1 && d.max2 >= d.max1)
        {
            Console.WriteLine($"{d.min1}-{d.max1} contained in {d.min2}-{d.max2}");
        }
        else
        {
            pt = 0;
        }
        total += pt;
    }
    Console.WriteLine(total);
}

static void part2()
{
    var total = 0;
    foreach (var line in File.ReadLines("in.txt"))
    {
        var match = Regex.Match(line, "(?<min1>\\d+)-(?<max1>\\d+),(?<min2>\\d+)-(?<max2>\\d+)");
        var d = new
        {
            min1 = int.Parse(match.Groups["min1"].Value),
            max1 = int.Parse(match.Groups["max1"].Value),
            min2 = int.Parse(match.Groups["min2"].Value),
            max2 = int.Parse(match.Groups["max2"].Value),
        };
        int lowerMax = d.max1;
        int upperMin = d.min2;
        if (d.min1 > d.min2)
        {
            lowerMax = d.max2;
            upperMin = d.min1;
        }

        if (lowerMax >= upperMin)
        {
            Console.WriteLine($"Overlap in {d.min1}-{d.max1}, {d.min2}-{d.max2}");
            total++;
        }
    }
    Console.WriteLine(total);
}

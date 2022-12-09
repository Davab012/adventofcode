using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

part2();

static void part1()
{
    var total = 0;
    foreach (var line in File.ReadLines("in.txt"))
    {
        var part1 = line.Substring(0, line.Length / 2);
        var part2 = line.Substring(line.Length / 2);
        var re = $"[{part1}]";
        var letterAscii = Regex.Match(part2, re).Value[0];
        var pt = letterAscii < 91 ? letterAscii - 65 + 27 : letterAscii - 97 + 1;
        total += pt;
//        Console.WriteLine($"Part1: {part1} Part2: {part2}");
    }
    Console.WriteLine(total);
}

static void part2()
{
    var total = 0;
    var fs = new StreamReader("in.txt");
    
    while(!fs.EndOfStream)
    {
        var line1 = fs.ReadLine();
        var line2 = fs.ReadLine()!;
        var line3 = fs.ReadLine()!;

        var re = $"[{line1}]";
        var allCommonLetters = Regex.Matches(line2, re);

        re = "[" + string.Join("", from match in allCommonLetters select match.Value) + "]";
        var letterAscii = Regex.Match(line3, re).Value[0];

        var pt = letterAscii < 91 ? letterAscii - 65 + 27 : letterAscii - 97 + 1;
        total += pt;
    }
    Console.WriteLine(total);
}

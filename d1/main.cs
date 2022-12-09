part2();

static void part1()
{
    var f = "in1.txt";
    Console.WriteLine("Hello, World!");
    Console.WriteLine();
    var max = 0;
    var curr = 0;
    foreach (var line in File.ReadLines(f))
    {
        if (string.IsNullOrEmpty(line))
        {
            curr = 0;
        }
        else
        {
            curr += int.Parse(line);
            if (curr > max)
            {
                max = curr;
            }
        }
    }
    Console.WriteLine("Max: " + max);
}
static void part2()
{
    var f = "in1.txt";
    var max = new int[3] { 0, 0, 0 };
    var curr = 0;
    foreach (var line in File.ReadLines(f))
    {
        if (string.IsNullOrEmpty(line))
        {
            for(var i = 2; i >= 0; i--)
            { 
                if (curr > max[i])
                {
                    for(var j = 0; j < i; j++)
                    {
                        max[j] = max[j + 1];
                    }
                    max[i] = curr;
                    break;
                }
            }
            curr = 0;
        }
        else
        {
            curr += int.Parse(line);
        }
    }
    Console.WriteLine("Max: " + max.Sum());
}

part2();

static int CalculateScore(int opponent, int myself)
{
    var pt = 0;
    // win
    if (myself == (opponent + 1) % 3)
    {
        pt += 6;
    }
    // draw
    else if (myself == opponent)
    {
        pt += 3;
    }
    // Points for shape chosen
    pt += myself + 1;
    //Console.WriteLine($"Calculating points for {opponent}:{myself} = {pt}");

    return pt;
}

static void part1()
{
    var combinations = new Dictionary<string, int>();
    // Create all possible combinations and points
    for (var opponent = 0; opponent < 3; opponent++)
    {
        for (var myself = 0; myself < 3; myself++)
        {
            var pt = 0;
            // win
            if (myself == (opponent + 1) % 3)
            {
                pt += 6;
            }
            // draw
            else if (myself == opponent)
            {
                pt += 3;
            }
            // Points for shape chosen
            pt += myself + 1;
            // What is the string that would be in the input? e.g. "A Z"
            var cString = "" + (char)(65 + opponent) + " " + (char)(88 + myself);
            Console.WriteLine(cString);
            combinations.Add(cString, pt);
        }
    }

    var total = 0;
    foreach (var line in File.ReadLines("in.txt"))
    {
        total += combinations[line];
    }
    Console.WriteLine(total);
}

static void part2()
{
    var combinations = new Dictionary<string, int>();
    // Create all possible combinations and points
    for (var opponent = 0; opponent < 3; opponent++)
    {
        for (var strategy = 0; strategy < 3; strategy++)
        {
            var myself = opponent;
            // win
            if (strategy == 2)
            {
                myself = (opponent + 1) % 3;
            }
            // loose
            else if (strategy == 0)
            {
                myself = (opponent + 2) % 3;
            }
            int pt = CalculateScore(opponent, myself);
            // What is the string that would be in the input? e.g. "A Z"
            var cString = "" + (char)(65 + opponent) + " " + (char)(88 + strategy);
            Console.WriteLine(cString);
            combinations.Add(cString, pt);
        }
    }

    var total = 0;
    foreach (var line in File.ReadLines("in.txt"))
    {
        total += combinations[line];
    }
    Console.WriteLine(total);
}

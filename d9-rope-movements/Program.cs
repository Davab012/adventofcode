// See https://aka.ms/new-console-template for more information
using d9_rope_movements;

//part1_rewritten("in.txt");
part2_rewritten_coord("in.txt");
part2("in.txt");

static void part1(string file)
{    
    int hx = 0, hy = 0, tx = 0, ty = 0;
    
    var fileLines = File.ReadAllLines(file);

    var visited = new Dictionary<string, int>
    {
        { $"{ty}:{tx}", 1 }
    };

    foreach (var l in fileLines)
    {
        int moveY = 0, moveX = 0;
        var parts = l.Split(" ");
        var steps = int.Parse(parts[1]);
        switch (parts[0])
        {
            case "U":
                moveY = 1;
                break;
            case "D":
                moveY = -1;
                break;
            case "L":
                moveX = -1;
                break;
            case "R":
                moveX = 1;
                break;
            default:
                throw new Exception("Unknown direction " + parts[0]);
        }

        // Move head
        while (steps > 0)
        {
            hx += moveX;
            hy += moveY;
            
            // Calculate how tail would need to move to match head (distance), limit to 1 step
            var dx = hx - tx;
            var dy = hy - ty;

            // Needs to move tail?
            if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
            {
                // Move tail, limit to one step
                tx += dx != 0 ? dx / Math.Abs(dx) : 0;
                ty += dy != 0 ? dy / Math.Abs(dy) : 0;
            }

            // Add position to visited list
            var k = $"{ty}:{tx}";
            if (!visited.ContainsKey(k))
            {
                visited.Add($"{ty}:{tx}", 1);
            }

            steps--;
        }
    }
    Console.WriteLine($"Visited {visited.Count} places");
}

static void part1_rewritten_valuetuple(string file)
{
    (int x, int y) headPos = (0, 0);
    (int x, int y) tailPos = (0, 0);
    
    var fileLines = File.ReadAllLines(file);

    var visited = new HashSet<(int, int)>();
    visited.Add(tailPos);
    
    foreach (var l in fileLines)
    {
        int moveY = 0, moveX = 0;
        var parts = l.Split(" ");
        var steps = int.Parse(parts[1]);
        switch (parts[0])
        {
            case "U":
                moveY = 1;
                break;
            case "D":
                moveY = -1;
                break;
            case "L":
                moveX = -1;
                break;
            case "R":
                moveX = 1;
                break;
            default:
                throw new Exception("Unknown direction " + parts[0]);
        }

        // Move head
        while (steps > 0)
        {
            headPos.x += moveX;
            headPos.y += moveY;

            // Calculate how tail would need to move to match head (distance), limit to 1 step
            var dx = headPos.x - tailPos.x;
            var dy = headPos.y - tailPos.y;

            // Needs to move tail?
            if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
            {
                // Move tail, limit to one step
                tailPos.x += dx != 0 ? dx / Math.Abs(dx) : 0;
                tailPos.y += dy != 0 ? dy / Math.Abs(dy) : 0;
            }

            // Add position to visited list
            visited.Add(tailPos);

            steps--;
        }
    }
    Console.WriteLine($"Visited {visited.Count} places");
}

static void part1_rewritten_coord(string file)
{
    var headPos = new Coord(0, 0);
    var tailPos = new Coord(0, 0);

    var fileLines = File.ReadAllLines(file);

    var visited = new HashSet<Coord>();
    visited.Add(tailPos);

    foreach (var l in fileLines)
    {
        int moveY = 0, moveX = 0;
        var parts = l.Split(" ");
        var steps = int.Parse(parts[1]);
        switch (parts[0])
        {
            case "U":
                moveY = 1;
                break;
            case "D":
                moveY = -1;
                break;
            case "L":
                moveX = -1;
                break;
            case "R":
                moveX = 1;
                break;
            default:
                throw new Exception("Unknown direction " + parts[0]);
        }

        // Move head
        while (steps > 0)
        {
            headPos.Move(moveX, moveY);

            // Needs to move tail?
            if (tailPos.Distance1d(headPos) > 1)
            {
                // Calculate how tail would need to move to match head (distance), limit to 1 step
                var diff = tailPos.CalculateDiff(headPos);

                // Move tail, limit to one step
                tailPos.Move(diff, 1);
            }
            
            // Add position to visited list
            visited.Add(tailPos);

            steps--;
        }
    }
    Console.WriteLine($"Visited {visited.Count} places");
}

static void part2(string file)
{
    const int itemCount = 10;
    const int tailIndex = itemCount - 1;

    // Head is [0] and tail is [itemCount-1]
    var knotX = new int[itemCount];
    var knotY = new int[itemCount];

    var fileLines = File.ReadAllLines(file);

    var visited = new Dictionary<string, int>
    {
        { $"{knotY[tailIndex]}:{knotX[tailIndex]}", 1 }
    };

    foreach (var l in fileLines)
    {
        int moveY = 0, moveX = 0;
        var parts = l.Split(" ");
        var steps = int.Parse(parts[1]);
        
        switch (parts[0])
        {
            case "U":
                moveY = 1;
                break;
            case "D":
                moveY = -1;
                break;
            case "L":
                moveX = -1;
                break;
            case "R":
                moveX = 1;
                break;
            default:
                throw new Exception("Unknown direction " + parts[0]);
        }

        // Move head and all knots
        while (steps > 0)
        {
            knotX[0] += moveX;
            knotY[0] += moveY;

            // Move each knot
            for (var i = 1; i < itemCount; i++)
            {
                var hx = knotX[i - 1];
                var hy = knotY[i - 1];
                var tx = knotX[i];
                var ty = knotY[i];
 
                // Calculate how following knot would move
                var dx = hx - tx;
                var dy = hy - ty;

                // Needs to move tail?
                if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
                {
                    // Move tail, limit to one step
                    tx += dx != 0 ? dx / Math.Abs(dx) : 0;
                    ty += dy != 0 ? dy / Math.Abs(dy) : 0;
                }

                knotX[i] = tx;
                knotY[i] = ty;
            }

            // Add tail position to visited list
            var k = $"{knotY[tailIndex]}:{knotX[tailIndex]}";
            if (!visited.ContainsKey(k))
            {
                visited.Add(k, 1);
            }

            steps--;
        }

        // Display grid and knots
        //displayGrid(knotY, knotX, itemCount);
    }
    Console.WriteLine($"Visited {visited.Count} places");
}

static void part2_rewritten_coord(string file)
{
    const int itemCount = 10;
    const int tailIndex = itemCount - 1;

    var pos = Enumerable.Range(0, itemCount).Select(i => new Coord(0,0)).ToArray();
    
    var fileLines = File.ReadAllLines(file);

    var visited = new HashSet<Coord>();
    visited.Add(pos[tailIndex]);

    foreach (var l in fileLines)
    {
        int moveY = 0, moveX = 0;
        var parts = l.Split(" ");
        var steps = int.Parse(parts[1]);
        switch (parts[0])
        {
            case "U":
                moveY = 1;
                break;
            case "D":
                moveY = -1;
                break;
            case "L":
                moveX = -1;
                break;
            case "R":
                moveX = 1;
                break;
            default:
                throw new Exception("Unknown direction " + parts[0]);
        }

        // Move head
        while (steps > 0)
        {
            pos[0].Move(moveX, moveY);

            // Move the rest of the knots
            foreach (var i in Enumerable.Range(1, itemCount - 1))
            {
                if (pos[i].Distance1d(pos[i - 1]) > 1)
                {
                    // Calculate how following knot would move
                    var diff = pos[i].CalculateDiff(pos[i - 1]);

                    // Move, limit to one step
                    pos[i].Move(diff, 1);
                }
            }

            // Add tail position to visited list
            visited.Add(pos[tailIndex]);

            steps--;
        }
    }
    Console.WriteLine($"Visited {visited.Count} places");
}


static void displayGrid(int[] Y, int[] X, int cnt)
{
    // Calculate grid dimensions
    int maxX = 1, maxY = 1, minX = 1, minY = 1;
    for (var i = cnt - 1; i >= 0; i--)
    {
        maxX = Math.Max(maxX, X[i]);
        maxY = Math.Max(maxY, Y[i]);
        minX = Math.Min(minX, X[i]);
        minY = Math.Min(minY, Y[i]);
    }
    

    // Display grid
    for(var row = maxY; row >= minY; row--)
    {
        for (var col = minX; col <= maxX; col++)
        {
            var c = ".";
            for (var i = cnt-1; i >= 0; i--)
            {
                if (Y[i] == row && X[i] == col)
                {
                    c = i == 0 ? "H" : i == cnt - 1 ? "T" : i.ToString();
                }
            }
            Console.Write(c);
        }
        Console.WriteLine();
    }
    Console.WriteLine();

}
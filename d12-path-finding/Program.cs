part1("in.txt");

static void part1(string file)
{
    // Find the shortest path between a and b using the A* algorithm
    int startX = 0, startY = 0;
    int endX = 0, endY = 0;
    var fileLines = File.ReadAllLines(file);

    var width = fileLines[0].Length;
    var height = fileLines.Length;
    var matrix = new MatrixPoint[width, height];

    // Add all heights
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var mapHeight = fileLines[y][x];
            switch (mapHeight)
            {
                case 'S':
                    mapHeight = 'a';
                    startX = x;
                    startY = y;
                    break;
                case 'E':
                    mapHeight = 'z';
                    endX = x;
                    endY = y;
                    break;
            }

            matrix[x, y] = new MatrixPoint(x, y, (int)mapHeight - 97);
        }
    }
    // First attempt. Calculate the distance to every point.
    // Start at the beginning and always add all neighbours that have a higher cost
    // even if the have been visited before

    var currNode = matrix[startX, startY];
    currNode.DistanceToStart = 0;
    var openList = new List<MatrixPoint>();
    int iteration = 0;
    do
    {
        iteration++;
        // Look around (top, bottom, left, right) and add neighbours to check, if not already in open list
        // and if it would be cheaper to get there from this point.
        var neighbours = new List<MatrixPoint>();
        if (currNode.X > 0) neighbours.Add(matrix[currNode.X - 1, currNode.Y]);
        if (currNode.X < width - 1) neighbours.Add(matrix[currNode.X + 1, currNode.Y]);
        if (currNode.Y > 0) neighbours.Add(matrix[currNode.X, currNode.Y - 1]);
        if (currNode.Y < height - 1) neighbours.Add(matrix[currNode.X, currNode.Y + 1]);

        // Move to the best possible neighbour (take heights into account)
        foreach (var neighbour in neighbours)
        {
            if (neighbour.Height <= currNode.Height + 1 && neighbour.DistanceToStart > currNode.DistanceToStart + 1)
            {
                // Add to open list
                openList.Add(neighbour);
                neighbour.DistanceToStart = currNode.DistanceToStart + 1;
                neighbour.Parent = currNode;
            }
        }

        openList.Sort((a, b) => a.TaxicabDistanceToGoal(endX, endY) < b.TaxicabDistanceToGoal(endX, endY) ? -1 : 1);
        if (iteration % 100 == 0)
        {
            Console.Clear();
            DisplayMatrix(matrix, openList, currNode, endX, endY);
        }
        currNode = openList.First();
        if (currNode == null)
        {
            break;
        }
        openList.Remove(currNode);
        currNode.CheckedCount++;
        //    } while (currNode.X != endX || currNode.Y != endY);
    } while (openList.Count>0);
    Console.Clear();
    currNode = matrix[endX, endY];
    DisplayMatrix(matrix, openList, currNode, endX, endY);

    Console.WriteLine($"At end {currNode.X},{currNode.Y}, distance {currNode.DistanceToStart} in {iteration} iterations. Checked {currNode.CheckedCount} times");
}

static void part2(string file)
{
    // Start from the End and map coordinates on the map. We are only interested in the DistanceFromStart value
    // Once everything is mapped we can search for the coordinate with height a having the least distance.
    // Reverse the height-logic
    int startX = 0, startY = 0;
    var fileLines = File.ReadAllLines(file);

    var width = fileLines[0].Length;
    var height = fileLines.Length;
    var matrix = new MatrixPoint[width, height];
    var possibleAnswers = new List<MatrixPoint>();

    // Add all heights
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var mapHeight = fileLines[y][x];
            switch (mapHeight)
            {
                case 'S':
                    mapHeight = 'a';
                    break;
                case 'E':
                    mapHeight = 'z';
                    startX = x;
                    startY = y;
                    break;
            }

            matrix[x, y] = new MatrixPoint(x, y, (int)mapHeight - 97);
            if (matrix[x, y].Height == 0)
            {
                possibleAnswers.Add(matrix[x, y]);
            }
        }
    }
    // Calculate the distance to every point.
    // Start at the beginning/end and always add all neighbours that have a higher cost
    // even if the have been visited before

    var currNode = matrix[startX, startY];
    currNode.DistanceToStart = 0;
    var openList = new List<MatrixPoint>();
    int iteration = 0;
    do
    {
        iteration++;
        // Look around (top, bottom, left, right) and add neighbours to check, if not already in open list
        // and if it would be cheaper to get there from this point.
        var neighbours = new List<MatrixPoint>();
        if (currNode.X > 0) neighbours.Add(matrix[currNode.X - 1, currNode.Y]);
        if (currNode.X < width - 1) neighbours.Add(matrix[currNode.X + 1, currNode.Y]);
        if (currNode.Y > 0) neighbours.Add(matrix[currNode.X, currNode.Y - 1]);
        if (currNode.Y < height - 1) neighbours.Add(matrix[currNode.X, currNode.Y + 1]);

        // Move to the best possible neighbour (take heights into account)
        foreach (var neighbour in neighbours)
        {
            if (neighbour.Height >= currNode.Height - 1 && neighbour.DistanceToStart > currNode.DistanceToStart + 1)
            {
                // Add to open list
                openList.Add(neighbour);

                neighbour.DistanceToStart = currNode.DistanceToStart + 1;
                neighbour.Parent = currNode;
            }
        }
        
        if (iteration % 100 == 0)
        {
            Console.Clear();
            DisplayMatrix(matrix, openList, currNode, startX, startY);
        }
        if (openList.Count == 0)
        {
            break;
        }

        currNode = openList.First();
        openList.Remove(currNode);
        currNode.CheckedCount++;
        //    } while (currNode.X != endX || currNode.Y != endY);
    } while (true);

    // All points should now have been mapped. Search for a point with height a and the lowest distance
    possibleAnswers.Sort((a, b) => a.DistanceToStart < b.DistanceToStart ? -1 : 1);
    currNode = possibleAnswers.First();
    Console.Clear();
    
    DisplayMatrix(matrix, openList, currNode, startX, startY);
    
    Console.WriteLine($"Start point is at {currNode.X},{currNode.Y}, distance {currNode.DistanceToStart} in {iteration} iterations. Checked {currNode.CheckedCount} times");
}

static void DisplayMatrix(MatrixPoint[,] matrix, List<MatrixPoint> openList, MatrixPoint currNode, int endX, int endY)
{
    var width = matrix.GetLength(0);
    var height = matrix.GetLength(1);

    // Follow the parent trail from currNode to start, these points should have a different background color
    var trail = new List<MatrixPoint>();
    var trailNode = currNode;
    while(trailNode.Parent != null)
    {
        trail.Add(trailNode.Parent);
        trailNode = trailNode.Parent;
    }

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (trail.Contains(matrix[x, y]))
            {
                Console.BackgroundColor = ConsoleColor.Green;
            }
            if (matrix[x, y] == currNode)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
            }
            if (x == endX && y == endY)
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }
            if (matrix[x, y].CheckedCount > 0)
            {
                // Set foregroundcolor to RGB color: 10, 20, 30
                Console.ForegroundColor = ConsoleColor.Green;
                
            }
            if (openList.Contains(matrix[x, y]))
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
            }
            Console.Write((char)(matrix[x, y].Height+97));
            Console.ResetColor();
        }
        Console.WriteLine();
    }
}

public class MatrixPoint
{
    public MatrixPoint(int x, int y, int v)
    {
        X = x;
        Y = y;
        Height = v;
        DistanceToStart = int.MaxValue;
    }

    public int DistanceToStart { get; set; }
    public int Height{ get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public MatrixPoint Parent { get; set; }
    public int CheckedCount { get; set; }
    public int TaxicabDistanceToGoal(int goalX, int goalY)
    {
        return Math.Abs(X - goalX) + Math.Abs(Y - goalY); 
    }

}
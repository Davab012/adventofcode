using d7_file_structure;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

part1("in.txt");

static void part1(string file)
{
    // No files are added or removed so once a directory is ls'd
    // we know the content.
    int pos = 0;
    var allLines = File.ReadAllLines(file);
    var fileSizesInDir = new Dictionary<string, int>();
    var path = new Stack<string>();
    for (var currLine = 0; currLine < allLines.Length; currLine++)
    {
        var line = allLines[currLine];

        var match = Regex.Match(line, "\\$ (?<cmd>\\w+) ?(?<arg>.+)?");
        if (match.Success)
        {
            var cmd = match.Groups["cmd"].Value;
            var arg = match.Groups["arg"].Value;
            switch (cmd)
            {
                case "cd":
                    Console.WriteLine("cd " + arg);
                    if (arg == "..")
                    {
                        path.Pop();
                    }
                    else if (arg == "/")
                    {
                        path.Clear();
                    }
                    else
                    {
                        path.Push(arg);
                    }
                    break;
                case "ls":
                    // The file listing comes on the following lines, could be 0 or more. Read the file sizes, ignore the directories
                    var dirSize = 0;
                    while (currLine + 1 < allLines.Length && !allLines[currLine + 1].StartsWith("$"))
                    {
                        currLine++;
                        line = allLines[currLine];
                        match = Regex.Match(line, "(\\d+) ");
                        if (match.Success)
                        {
                            dirSize += int.Parse(match.Groups[1].Value);
                        }
                        else
                        {
                            //Console.WriteLine(line);
                        }
                    }
                    var dir = string.Join("/", path.Reverse());

                    //if (dirSize > 0)
                    //{
                        if (fileSizesInDir.ContainsKey(dir))
                        {
                            throw new Exception("Trying to add a directory that already exists");
                        }
                        fileSizesInDir[dir] = dirSize;
                    //}
                    Console.WriteLine($"ls at directory {dir}, size {dirSize}");
                    break;
            }
        }
        else
        {
            throw new Exception("Unknown command");
        }


    }
    
    // Now we have all directories and respective sizes in the dirSizes dictionary,
    // Print the size of a directory and all it's subdirectories
    var totalsLessThan100k = 0;
    var runningTotal = 0;
    var totalDirSizes = new Dictionary<string, int>();
    foreach (var dir in fileSizesInDir.Keys)
    {
        var size = fileSizesInDir[dir];
        runningTotal += size;
        
        foreach (var subDir in fileSizesInDir.Keys.Where(k => k.StartsWith(dir+"/")))
        {
          //  Console.WriteLine(" - " + subDir + " " + fileSizesInDir[subDir]);
            size += fileSizesInDir[subDir];
        }
        //Console.WriteLine(" = " + size);
        totalDirSizes[dir] = size;
        if (size <= 100000)
        {
//            Console.WriteLine(dir + " " + size);
            totalsLessThan100k+=size;
        };
    }
    Console.WriteLine($"Total directory size with size less than 100k: {totalsLessThan100k}");
    Console.WriteLine($"The total size of all files in all directories are {runningTotal}");
    
    foreach (var dir in totalDirSizes.Keys.OrderBy(k => k))
    {
        var size = totalDirSizes[dir];
        if (size <= 100000)
        {
            Console.WriteLine($"{dir} {totalDirSizes[dir]}");
            runningTotal += size;
        }
    }

    Console.WriteLine($"The total size of all files < 100 000 in all directories are {runningTotal}");
}

static void part1_as_tree(string file)
{
    int pos = 0;
    var allLines = File.ReadAllLines(file);
    var fileSizesInDir = new Dictionary<string, int>();
    var path = new Stack<string>();
    var rootNode = new TreeNode("root");
    var currNode = rootNode;
    for (var currLine = 0; currLine < allLines.Length; currLine++)
    {
        var line = allLines[currLine];

        var match = Regex.Match(line, "\\$ (?<cmd>\\w+) ?(?<arg>.+)?");
        if (match.Success)
        {
            var cmd = match.Groups["cmd"].Value;
            var arg = match.Groups["arg"].Value;
            switch (cmd)
            {
                case "cd":

                    Console.WriteLine("cd " + arg);
                    if (arg == "..")
                    {
                        currNode = currNode.Parent != null ? currNode.Parent : currNode;
                    }
                    else if (arg == "/")
                    {
                        currNode = rootNode;
                    }
                    else
                    {
                        currNode = currNode.Children.Where(c => c.Name == arg).First();
                    }
                    break;
                case "ls":
                    // The file listing comes on the following lines, could be 0 or more. Read the file sizes, create the directories
                    var dirSize = 0;
                    while (currLine + 1 < allLines.Length && !allLines[currLine + 1].StartsWith("$"))
                    {
                        currLine++;
                        line = allLines[currLine];
                        var parts = line.Split(" ");
                        int size;
                        if (int.TryParse(parts[0], out size))
                        {
                            dirSize += size;
                        }
                        else if (parts[0] == "dir")
                        {
                            if (currNode.Children.Any(c => c.Name == parts[1]))
                            {
                                throw new Exception("Trying to add a directory that already exists");
                            }
                            currNode.AddChild(new TreeNode(parts[1]));
                        }
                    }

                    currNode.Size = dirSize;
                    break;
            }
        }
        else
        {
            throw new Exception("Unknown command");
        }
    }

    // Print the tree starting with the rootNode
    rootNode.Print();

    // Get all nodes having a total size of 100 000 or less.
    var nodes = rootNode.GetNodesWithSizeLessThan(100000);
    foreach (var node in nodes)
    {
        Console.WriteLine(node.GetPath() + " " + node.GetTotalSize());
    }

    // Get total size
    var totalSize = nodes.Sum(n => n.GetTotalSize());
    Console.WriteLine(totalSize);
}

static void part2_as_tree(string file)
{
    int pos = 0;
    var allLines = File.ReadAllLines(file);
    var fileSizesInDir = new Dictionary<string, int>();
    var path = new Stack<string>();
    var rootNode = new TreeNode("root");
    var currNode = rootNode;
    for (var currLine = 0; currLine < allLines.Length; currLine++)
    {
        var line = allLines[currLine];

        var match = Regex.Match(line, "\\$ (?<cmd>\\w+) ?(?<arg>.+)?");
        if (match.Success)
        {
            var cmd = match.Groups["cmd"].Value;
            var arg = match.Groups["arg"].Value;
            switch (cmd)
            {
                case "cd":

                    Console.WriteLine("cd " + arg);
                    if (arg == "..")
                    {
                        currNode = currNode.Parent != null ? currNode.Parent : currNode;
                    }
                    else if (arg == "/")
                    {
                        currNode = rootNode;
                    }
                    else
                    {
                        currNode = currNode.Children.Where(c => c.Name == arg).First();
                    }
                    break;
                case "ls":
                    // The file listing comes on the following lines, could be 0 or more. Read the file sizes, create the directories
                    var dirSize = 0;
                    while (currLine + 1 < allLines.Length && !allLines[currLine + 1].StartsWith("$"))
                    {
                        currLine++;
                        line = allLines[currLine];
                        var parts = line.Split(" ");
                        int size;
                        if (int.TryParse(parts[0], out size))
                        {
                            dirSize += size;
                        }
                        else if (parts[0] == "dir")
                        {
                            if (currNode.Children.Any(c => c.Name == parts[1]))
                            {
                                throw new Exception("Trying to add a directory that already exists");
                            }
                            currNode.AddChild(new TreeNode(parts[1]));
                        }
                    }

                    currNode.Size = dirSize;
                    break;
            }
        }
        else
        {
            throw new Exception("Unknown command");
        }
    }

    // Print the tree starting with the rootNode
    rootNode.Print();

    // How much space is needed?
    var neededSpace = 30000000 - (70000000 - rootNode.GetTotalSize());
    Console.WriteLine("Needed space: " + neededSpace);

    // Filter and sort directories that are bigger or equal in size to the neededSpace
    var nodes = rootNode.GetNodesWithSizeMoreThan(neededSpace).OrderBy(n => n.GetTotalSize()).ToList();
    
    foreach (var node in nodes)
    {
        Console.WriteLine(node.GetPath() + " " + node.GetTotalSize());
    }
}
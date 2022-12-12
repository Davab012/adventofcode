// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var line = File.ReadAllLines("sample.txt");

/* Sample 
   List<Monkey> monkeys = new List<Monkey>();
monkeys.Add(new Monkey("Monkey 0", new List<int>() { 79, 98 }, (x => x * 19), 23));
monkeys.Add(new Monkey("Monkey 1", new List<int>() { 54, 65, 75, 74 }, (x => x + 6), 19));
monkeys.Add(new Monkey("Monkey 2", new List<int>() { 79, 60, 97 }, (x => x * x), 13));
monkeys.Add(new Monkey("Monkey 3", new List<int>() { 74 }, (x => x + 3), 17));

monkeys[0].ThrowIfTrue = monkeys[2];
monkeys[0].ThrowIfFalse = monkeys[3];
monkeys[1].ThrowIfTrue = monkeys[2];
monkeys[1].ThrowIfFalse = monkeys[0];
monkeys[2].ThrowIfTrue = monkeys[1];
monkeys[2].ThrowIfFalse = monkeys[3];
monkeys[3].ThrowIfTrue = monkeys[0];
monkeys[3].ThrowIfFalse = monkeys[1];
*/


// Ugly and dirty

List<Monkey> monkeys = new List<Monkey>();
for(int i = 0; i < line.Length / 7 ; i ++)
{
    monkeys.Add(new Monkey("Monkey " + i));
}

// Create monkey from text
int curr = 0;
foreach(var m in monkeys)
{ 
    var name = line[curr++];
    Console.WriteLine("Creating monkey" + name);
    m.Items = Regex.Matches(line[curr++], "\\d+").Select(x => int.Parse(x.Value)).ToList();
    curr++;
    m.DivisableByTest = int.Parse(Regex.Match(line[curr++], "\\d+").Value);
    
    var throwIfTrue = int.Parse(Regex.Match(line[curr++], "\\d+").Value);
    m.ThrowIfTrue = monkeys[throwIfTrue];
    
    var throwIfFalse = int.Parse(Regex.Match(line[curr++], "\\d+").Value);
    m.ThrowIfFalse = monkeys[throwIfFalse];
    curr++;
}


for (var round = 0; round < 20; round ++)
{
    foreach (var monkey in monkeys)
    {
        Console.WriteLine();
        Console.WriteLine(monkey.Name);
        monkey.InspectAndThrow();
    }
}

foreach (var monkey in monkeys)
{
    Console.WriteLine(monkey);
}

class Monkey
{
    public List<int> Items;
    public int InspectionCount{ get; set; }
    public Func<int, int> Operation { get; }
    public int DivisableByTest { get; set; }
    public Monkey ThrowIfTrue { get; set; }
    public Monkey ThrowIfFalse { get; set; }
    public string Name{ get; set; }

    public Monkey(string name, List<int> startingList, Func<int, int> operation, int divisableByTest)
    {
        Name = name;
        Items = startingList;
        Operation = operation;
        DivisableByTest = divisableByTest;
    }
    public Monkey(string name)
    {
        Name = name;
        Items = new List<int>();
    }

    public void Catch(int item)
    {
        Console.WriteLine(Name + " caught " + item);
        Items.Add(item);
    }

    public override string ToString() => Name + ": " + String.Join(",", Items) + ". Inspected items " + InspectionCount + " times";

    public void InspectAndThrow()
    {
        foreach (var item in Items)
        {
            InspectionCount++;
            Console.WriteLine("Inspects item with worry level " + item);
            var result = (int)Math.Floor(Operation(item) / 3.0);
            if (result % DivisableByTest == 0)
            {
                ThrowIfTrue.Catch(result);
            }
            else
            {
                ThrowIfFalse.Catch(result);
            }
        }
        Items = new List<int>();
    }
}
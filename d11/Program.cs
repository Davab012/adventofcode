// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var line = File.ReadAllLines("sample.txt");

var monkeyCount = line.Count((string l) => Regex.IsMatch(l, "Monkey \\d:"));

/* Sample 
   List<Monkey> monkeys = new List<Monkey>();
monkeys.Add(new Monkey("Monkey 0", new List<long>() { 79, 98 }, (x => x * 19), 23));
monkeys.Add(new Monkey("Monkey 1", new List<long>() { 54, 65, 75, 74 }, (x => x + 6), 19));
monkeys.Add(new Monkey("Monkey 2", new List<long>() { 79, 60, 97 }, (x => x * x), 13));
monkeys.Add(new Monkey("Monkey 3", new List<long>() { 74 }, (x => x + 3), 17));

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
for (long i = 0; i < monkeyCount; i++)
{
    monkeys.Add(new Monkey("Monkey " + i));
}

// Create monkey from text
long curr = 0;
long commonDenominator = 1;
foreach (var m in monkeys)
{
    var name = line[curr++];
    Console.WriteLine("Creating monkey" + name);
    m.Items = Regex.Matches(line[curr++], "\\d+").Select(x => long.Parse(x.Value)).ToList();

    var operation = Regex.Match(line[curr++], "new = old ([+*]) (old|\\d+)");
    m.OperationOperator = operation.Groups[1].Value;
    m.OperationOperand = operation.Groups[2].Value;

    m.DivisableByTest = int.Parse(Regex.Match(line[curr++], "\\d+").Value);
    commonDenominator *= m.DivisableByTest;

    var throwIfTrue = int.Parse(Regex.Match(line[curr++], "\\d+").Value);
    m.ThrowIfTrue = monkeys[throwIfTrue];

    var throwIfFalse = int.Parse(Regex.Match(line[curr++], "\\d+").Value);
    m.ThrowIfFalse = monkeys[throwIfFalse];
    curr++;
}

foreach (var m in monkeys)
{
    m.CommonDenominator = commonDenominator;
}

for (var round = 0; round < 10000; round++)
{
    foreach (var monkey in monkeys)
    {
        monkey.InspectAndThrow();
    }
    if (round == 0 || round == 19 || round % 1000 == 0)
    {
        Console.WriteLine();
        Console.WriteLine($"== After round {round+1} ==");
        foreach (var monkey in monkeys)
        {
            Console.WriteLine(monkey.Name + " inspected  items " + monkey.InspectionCount + " times");
        }

    }

}

monkeys.Sort((a, b) => a.InspectionCount > b.InspectionCount ? -1 : 1);

foreach (var monkey in monkeys)
{
    Console.WriteLine(monkey);
}
Console.WriteLine("Multiplied inspection count: " + monkeys[0].InspectionCount * monkeys[1].InspectionCount);

class Monkey
{
    public List<long> Items;
    public long InspectionCount { get; set; }
    public Func<long, long> Operation { get; }
    public long DivisableByTest { get; set; }
    public string OperationOperator { get; set; }
    public string OperationOperand { get; set; }
    public Monkey ThrowIfTrue { get; set; }
    public Monkey ThrowIfFalse { get; set; }
    public string Name { get; set; }
    // All monkeys division test factors multiplied.
    public long CommonDenominator{ get; set; }

    public Monkey(string name)
    {
        Name = name;
        Items = new List<long>();
    }

    public void Catch(long item)
    {
        Items.Add(item);
    }

    public override string ToString() => Name + ": " + String.Join(",", Items) + ". Inspected items " + InspectionCount + " times";

    public long PerformOperation(long item)
    {
        var operand = OperationOperand == "old" ? item : long.Parse(OperationOperand);
        if (OperationOperator == "+")
        {
            return item + operand;
        }
        else
        {
            return item * operand;
        }
    }

    public void InspectAndThrow()
    {
        foreach (var item in Items)
        {
            InspectionCount++;
            //Console.WriteLine("Inspects item with worry level " + item);
            //var result = (long)Math.Floor(PerformOperation(item) / 3.0);
            var result = (long)(PerformOperation(item));
            
            if (result % DivisableByTest == 0)
            {
                // Since I know that all divisable checks are primes and we don't use the worry level
                // we can safely replace the worry level with the prime if it matches
                // 
                // But is this true?
                // 38 is divisable by 2, down to 19, but I cannot replace the worry level
                // with 2, since 38 would be divisable by 19.
                // I could however replace it with the result of the division: 19
                // 
                // Let's try it.
                
                do
                {
                    result /= DivisableByTest;
                } while (result % DivisableByTest == 0);
                
                ThrowIfTrue.Catch(result % DivisableByTest);
            }
            else
            {
                ThrowIfFalse.Catch(result);
            }
        }
        Items = new List<long>();
    }
}
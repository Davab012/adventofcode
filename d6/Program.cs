using System.Text;
using System.Text.RegularExpressions;

part2("in.txt");

static void part1(string file)
{
    var q = new Queue<int>();
    var fr = new StreamReader(file);

    int pos = 0;
    // The queue only contains unique characters. Once a non-unique character is found,
    // remove all characters up to this one.
    while(!fr.EndOfStream)
    {
        pos++;
        var c = fr.Read();
        if (q.Contains(c))
        {
            // Remove all characters from the queue up to and including the one just read.
            int removed = 0;
            do
            {
                removed = q.Dequeue();
            } while (removed != c);
        }else if (q.Count == 3)
        {
            q.Enqueue(c);
            Console.WriteLine("Found the marker! End of marker at " + pos);
            foreach(var ch in q)
            {
                Console.Write((char)ch);
            }
            break;
        }

        q.Enqueue(c);
    }
    Console.WriteLine("Ending with pos " + pos);
}

static void part2(string file)
{
    var q = new Queue<int>();
    var fr = new StreamReader(file);

    int pos = 0;
    // The queue only contains unique characters. Once a non-unique character is found,
    // remove all characters up to this one.
    while (!fr.EndOfStream)
    {
        pos++;
        var c = fr.Read();
        if (q.Contains(c))
        {
            // Remove all characters from the queue up to and including the one just read
            int removed = 0;
            do
            {
                removed = q.Dequeue();
            } while (removed != c);
        }
        else if (q.Count == 13)
        {
            q.Enqueue(c);
            Console.WriteLine("Found the marker! End of marker at " + pos);
            foreach (var ch in q)
            {
                Console.Write((char)ch);
            }

            // Loop through all items in the queue
            

            
            break;
        }

        q.Enqueue(c);
    }
    
    Console.WriteLine("Ending with pos " + pos);
}


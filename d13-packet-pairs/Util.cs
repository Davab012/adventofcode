using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d13_packet_pairs
{
    public class Util
    {
        public static int Compare(string leftInput, string rightInput)
        {
            Log($"Currently working with \n{leftInput} vs \n{rightInput}");
            var leftElements = Elementize(leftInput);
            var rightElements = Elementize(rightInput);
            int retVal = -1;
            leftElements.ForEach(e => Log($"Element {e}"));

            // Now we can compare the elements
            for (int elemIndex = 0; elemIndex < leftElements.Count; elemIndex++)
            {
                // If the element is a list, recurse
                var left = leftElements[elemIndex];
                var right = elemIndex < rightElements.Count ? rightElements[elemIndex] : null;

                Log($"{elemIndex}: left={left} right={right}");

                // left is empty, for example when Elementizing []
                if (left == null)
                {
                    if (right != null)
                    {
                        Log("Left ran out of elements, is in correct order");
                        return -1;
                    }
                }

                // right ran out of elements, not in correct order
                if (right == null)
                {
                    Log("Right ran out of elements, is not in correct order");
                    return 1;
                }

                if (!IsList(left) && !IsList(right))
                {
                    if (int.Parse(left) > int.Parse(right))
                    {
                        // Left is bigger
                        Log($"Left is bigger, {left} > {right}");
                        return 1;
                    }
                    else if (int.Parse(left) < int.Parse(right))
                    {
                        // Right is bigger
                        Log($"Right is bigger, {left} < {right}");
                        return -1;
                    }
                }
                else if (IsList(left) != IsList(right))
                {
                    if (IsList(left))
                    {
                        //var l = Elementize(left);
                        right = MakeList(right);
                    }
                    else
                    {
                        //var l = Elementize(right);
                        left = MakeList(left);
                    }
                }

                // Ugly.
                if (IsList(left))
                {
                    Log($"Left is a list left:{left} right:{right}");
                    retVal = Compare(left, right);
                    if (retVal != 0)
                    {
                        return retVal;
                    }
                }
            }

            Log($"Left ran out of elements, if right still has elements we're in correct order, otherwise we don't know");
            if (rightElements.Count > leftElements.Count)
            {
                return -1;
            }
            return 0;
        }

        static bool IsList(string input) => input != null && input != "" && input[0] == '[';


        // Comparing [2,3,4] with 4 should not result in right side running out of elements... strangely enough...
        // Solve this by duplicating the 4 to [4,4,4]
        static string MakeList(string input)//, int numDuplicates)
        {
            //var list = Enumerable.Repeat(input, numDuplicates);
            return '[' + input + ']'; // + string.Join(",", list) + ']';
        }

        public static void Log(string msg)
        {
            //Console.WriteLine(msg);
        }

        static List<string> Elementize(string str)
        {
            var parts = new List<string>();
            var level = 0;
            var i = 0;
            var previousEnd = -1;
            if (str[0] != '[')
            {
                parts.Add(str);
                return parts;
            }

            // Remove the first [
            str = str.Substring(1, str.Length - 2);
            if (str.Length == 0)
            {
                // Empty array, []
                return parts;
            }

            for (; i < str.Length; i++)
            {
                var c = str[i];
                if (c == '[')
                {
                    // Start of a new group, level up
                    level++;
                }
                if (c == ']')
                {
                    // End of a new group, level down
                    level--;
                }
                else if (c == ',')
                {
                    // Separator, extract element if we're at level 0 
                    if (level == 0)
                    {
                        var val = str.Substring(previousEnd + 1, i - (previousEnd + 1));
                        if (val == "") val = null;
                        parts.Add(val);
                        previousEnd = i;
                    }
                }
                else
                {
                    // Value, do nothing
                }
            }

            // End of string
            var val2 = str.Substring(previousEnd + 1);
            if (val2 == "") val2 = null;
            parts.Add(val2);

            return parts;
        }
    }
}

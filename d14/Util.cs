using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace d14
{
    public class Util
    {
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
    
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int[,] Tiles { get; set; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            MinX = 0;
            MinY = 0;
            Tiles = new int[Width, Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Tiles[x, y] = (int)'.';
                }
            }
          
            Console.SetBufferSize(Math.Max(Console.BufferWidth, Width), Math.Max(Console.BufferHeight, Height));
        }

        public void DrawLine(int fromX, int fromY, int toX, int toY, int value)
        {
            var dx = toX - fromX != 0 ? (toX - fromX) / Math.Abs(toX - fromX): 0;
            var dy = toY - fromY != 0 ? (toY - fromY) / Math.Abs(toY - fromY): 0;
            var steps = Math.Abs(toX - fromX) + Math.Abs(toY - fromY) + 1;
            for (var i = 0; i < steps; i++)
            {
                SetTile(fromX + dx * i, fromY + dy * i, value);
            }
        }

        public void SetTile(int x, int y, int value)
        {
            Tiles[x - MinX, y - MinY] = value;
            //DisplayTile(x, y);
        }

        public void DisplayTile(int x, int y)
        {
            DisplayChar(x, y, (char)Tiles[x - MinX, y - MinY]);
        }
        public void DisplayChar(int x, int y, char value)
        {
            Console.SetCursorPosition(x - MinX, y - MinY);
            Console.Write(value);
        }

        public void Display()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write((char)Tiles[x, y]);
                }
                Console.WriteLine();
            }
        }

        public char GetTile(int x, int y)
        {
            return (char) Tiles[x - MinX, y - MinY];
        }
    }

    public class Simulator
    {
        public Map Map { get; set; }

        public int SandX { get; set; }
        public int SandY { get; set; }


        // Returns true if the sand is at rest when the simulation loop ends
        public bool Run(int msDelay = 0, bool display = true)
        {
            var isAtRest = false;
            var oldX = SandX;
            var oldY = SandY;
            while (!isAtRest)
            {
                if (display)
                { 
                    Map.DisplayChar(oldX, oldY, '.');                
                    Map.DisplayChar(SandX, SandY, '*');
                }
                oldX = SandX;
                oldY = SandY;

                // Move sand one step
                try
                {
                    if (Map.GetTile(SandX, SandY + 1) == '.')
                    {
                        SandY++;
                    }
                    else if (Map.GetTile(SandX - 1, SandY + 1) == '.')
                    {
                        SandY++;
                        SandX--;
                    }
                    else if (Map.GetTile(SandX + 1, SandY + 1) == '.')
                    {
                        SandY++;
                        SandX++;
                    }
                    else
                    {
                        isAtRest = true;
                    }
                }catch
                {
                    // outside map... lol.
                    return false;
                }

                if(msDelay > 0)
                {
                    Thread.Sleep(msDelay);
                }
            }
            
            Map.SetTile(SandX, SandY, 'o');
            if (display)
            {
                Map.DisplayChar(SandX, SandY, 'o');
            }
            return true;
        }
    }
}

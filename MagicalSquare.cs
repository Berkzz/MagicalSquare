using System;
using System.Diagnostics;

namespace MagicalSquare
{
    class MagicalSquare
    {
        static void Main()
        {
            Square sq = new Square(int.Parse(Console.ReadLine()));
            sq.Solve();
            sq.Check();
            Output.PrintInFile(sq);
            Process.Start("notepad.exe", "output.txt");
            Console.ReadLine();
        }
    }
}

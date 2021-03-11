using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MagicalSquare
{
    class Output
    {
        private static string Format(string str, int length)
        {
            int freeChars = length - str.Length + 2;
            while(freeChars > 0)
            {
                if (freeChars % 2 == 0)
                {
                    str = str + " ";
                } else
                {
                    str = " " + str;
                }
                freeChars--;
            }
            return str;
        }

        public static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1) ; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public static void PrintInFile(Square square)
        {
            int n = square.N;
            int maxLength = (n * n).ToString().Length;
            int[,] matrix = square.Matrix;
            string temp = "+";
            string hor = "";
            for (int i = 0; i < maxLength + 2; i++)
            {
                hor += "-";
            }
            for (int i = 0; i < n; i++)
            {
                temp += hor + "+";
            }

            string[] strings = new string[3 * n];
            string str = "";

            for(int i = 0; i < n; i++)
            {
                str += "\n";
                str += temp;
                str += "\n";
                str += "|";
                for (int j = 0; j < n; j++)
                {
                     str += Format(matrix[j,i].ToString(), maxLength) + "|";
                }
            }
            str += "\n";
            str += temp;
            File.WriteAllText("output.txt", str);
        }
    }
}

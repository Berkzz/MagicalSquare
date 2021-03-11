using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MagicalSquare
{
    class Square
    {
        public int[,] Matrix { get; }
        public int N { get; }
        private int M {get;}
        private Point coords;

        private void Init(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] = -1;
        }


        public Square (int N)
        {
            Matrix = new int[N, N];
            this.N = N;
            M = (this.N * (this.N * this.N + 1)) / 2;
            Init(Matrix);
        }

        private int Insert (Point point, int number, int[,] matrix)
        {
            if (matrix[point.X, point.Y] == -1) 
            {
                matrix[point.X, point.Y] = number;
                return 1;
            } else
                return 0;
        }

        private Point Offset(Point point, Point step, int size)
        {
            point.X = point.X + step.X;
            point.Y = point.Y + step.Y;
            if (point.X >= size) 
                point.X = point.X % size;
            if (point.Y >= size)
                point.Y = point.Y % size;
            if (point.X < 0)
                point.X = Math.Abs(size + point.X) % size;
            if (point.Y < 0)
                point.Y = Math.Abs(size + point.Y) % size;
            return point;
        }

        public static int[,] Eject(int[,] matrix, int x, int y, int height, int width)
        {
            int[,] result = new int[width, height];
            for (int i = x; i < x + width; i++)
                for(int j = y; j < y + height; j++)
                    result[i - x, j - y] = matrix[i, j];
            return result;
        }

        public static int[,] Replace(int[,] matrix, int[,] ejectMatrix, int x, int y)
        {
            for (int i = x; i < x + ejectMatrix.GetLength(0) ; i++)
                for (int j = y; j < y + ejectMatrix.GetLength(1) ; j++)
                    matrix[i, j] = ejectMatrix[i - x, j - y];
            return matrix;
        }

        private int SolveOdd(int[,] matrix, int startNumber)
        {
            int n = matrix.GetLength(0);
            coords = new Point(n / 2 , 0);
            Point prev = new Point(0, 0);
            int total = startNumber;
            int i;
            while (total < startNumber + n*n)
            {
                i = Insert(coords, total, matrix);
                if (i == 0)
                {
                    prev = Offset(prev, new Point(0, 1), n);
                    coords = prev;
                    continue;
                } else
                {
                    prev = coords;
                    total++;
                    coords = Offset(coords, new Point(1, -1), n);
                }
            }
            return total;
        }

        private void SolveMod2(int[,] matrix, int startNumber)
        {
            int[,] newMatrix = new int[matrix.GetLength(0) / 2, matrix.GetLength(1) / 2];
            for (int i = 0; i < 4; i++)
            {
                Init(newMatrix);
                SolveOdd(newMatrix, startNumber + i * newMatrix.Length);
                switch (i)
                {
                    case 0:
                        Replace(matrix, newMatrix, 0, 0);
                        break;
                    case 1:
                        Replace(matrix, newMatrix, matrix.GetLength(0) / 2, matrix.GetLength(1) / 2);
                        break;
                    case 2:
                        Replace(matrix, newMatrix, matrix.GetLength(0) / 2, 0);
                        break;
                    case 3:
                        Replace(matrix, newMatrix, 0, matrix.GetLength(1) / 2);
                        break;

                }
            }
            // some replacements... sorry
            // refactor pls..
            int[,] replaceMatrix = Eject(matrix, 0, 0, newMatrix.GetLength(0) / 2, newMatrix.GetLength(1) / 2);
            int[,] tempMatrix = Eject(matrix, 0, matrix.GetLength(1) / 2, newMatrix.GetLength(0) / 2, newMatrix.GetLength(1) / 2);
            Replace(matrix, replaceMatrix, 0, matrix.GetLength(1) / 2);
            Replace(matrix, tempMatrix, 0, 0);
            replaceMatrix = Eject(matrix, 1, newMatrix.GetLength(1) / 2, 1, newMatrix.GetLength(0) / 2);
            tempMatrix = Eject(matrix, 1, newMatrix.GetLength(1) / 2 + matrix.GetLength(1) / 2, 1, newMatrix.GetLength(0) / 2);
            Replace(matrix, replaceMatrix, 1, newMatrix.GetLength(0) / 2 + matrix.GetLength(1) / 2);
            Replace(matrix, tempMatrix, 1, newMatrix.GetLength(0) / 2);
            replaceMatrix = Eject(matrix, 0, newMatrix.GetLength(1) / 2 + 1, newMatrix.GetLength(0) / 2, newMatrix.GetLength(0) / 2);
            tempMatrix = Eject(matrix, 0, newMatrix.GetLength(1) / 2 + matrix.GetLength(1) / 2 + 1, 1, newMatrix.GetLength(0) / 2);
            Replace(matrix, replaceMatrix, 0, newMatrix.GetLength(0) / 2 + matrix.GetLength(1) / 2 + 1);
            Replace(matrix, tempMatrix, 0, newMatrix.GetLength(0) / 2 + 1);
            replaceMatrix = Eject(matrix, matrix.GetLength(0) - (newMatrix.GetLength(0) / 2) + 1, 0, matrix.GetLength(0) / 2, newMatrix.GetLength(0) / 2 - 1);
            tempMatrix = Eject(matrix, matrix.GetLength(0) - newMatrix.GetLength(0) / 2 + 1, matrix.GetLength(1) / 2, matrix.GetLength(0) / 2, newMatrix.GetLength(0) / 2 - 1);
            Replace(matrix, replaceMatrix, matrix.GetLength(0) - newMatrix.GetLength(0) / 2 + 1, matrix.GetLength(1) / 2);
            Replace(matrix, tempMatrix, matrix.GetLength(0) - (newMatrix.GetLength(0) / 2) + 1, 0);

        }

        private void SolveMod24(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int cornerN = matrix.GetLength(0) / 4;
            int number = 1;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i <= cornerN - 1 || i >= n - cornerN)
                    {
                        if (j <= cornerN - 1 || j >= n - cornerN)
                            matrix[j, i] = n * n - number + 1;
                        else
                            matrix[j, i] = number;
                    } else
                        if (j <= cornerN - 1 || j >= n - cornerN)
                            matrix[j, i] = number;
                        else
                            matrix[j, i] = n * n - number + 1;
                    number++;
                }
            }
        }

        public void Check()
        {
            int err = 0;
            int sum = 0;
            Console.WriteLine("Checking...");
            // vertical
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    sum += Matrix[i, j];
                if (M != sum) err++;
                sum = 0;
            }
            //horizontal
            for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < N; i++)
                    sum += Matrix[i, j];
                if (M != sum) err++;
                sum = 0;
            }
            //maindiagonal
            for (int i = 0; i < N; i++)
                sum += Matrix[i, i];
            if (M != sum) err++;
            sum = 0;
            //reversediagonal
            for (int i = 0; i < N; i++)
                sum += Matrix[i, N - i - 1];
            if (M != sum) err++;
            sum = 0;
            if (err == 0)
                Console.WriteLine("There is no errors!");
            else
                Console.WriteLine("Something went wrong!");
        }

        public void Solve()
        {
            if (N == 2)
            {
                Console.WriteLine("n = 2 magical square doesn't exist");
                return;
            }
            Console.WriteLine("Magical constant = {0}", M);
            if (N % 2 == 0)
            {
                if (N % 4 == 0)
                    SolveMod24(Matrix);
                else
                    SolveMod2(Matrix, 1);
            } else
                SolveOdd(Matrix, 1);
        }

        public void Print()
        {
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                    Console.Write(Matrix[j, i] + "\t");
                Console.WriteLine();
            }
        }
    }
}

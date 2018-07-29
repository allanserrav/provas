using System;
using System.Collections.Generic;
using System.Linq;

namespace NasajonTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            //int smallest = solutionTest(new int[] { 1, 2, 3 });
            //int r = solutionRealAMod(3, new int[] { 1, 2, 4, 3, 4, 3, 1, 2, 2, 1, 4 });
            int r = solutionRealB("13 DUP 4 POP 5 DUP + DUP + -");
            Console.WriteLine(r);
            Console.ReadKey();
        }

        public static int solutionRealB(string S)
        {
            var commands = S.Split(" ");
            var pilha = new Stack<int>();
            try
            {
                foreach (string command in commands)
                {
                    switch (command.ToUpper())
                    {
                        case "POP":
                            pilha.Pop();
                            break;
                        case "DUP":
                            int last = pilha.Pop();
                            pilha.Push(last);
                            pilha.Push(last);
                            break;
                        case "+":
                            int op1add = pilha.Pop();
                            int op2add = pilha.Pop();
                            pilha.Push(op1add + op2add);
                            break;
                        case "-":
                            int op1sub = pilha.Pop();
                            int op2sub = pilha.Pop();
                            pilha.Push(op1sub - op2sub);
                            break;
                        default:
                            int n = 0;
                            if (!int.TryParse(command, out n))
                            {
                                return -1; // ERRO
                            }
                            pilha.Push(n);
                            break;
                    }
                }
                return pilha.Pop();
            }
            catch
            {
                return -1;
            }
        }

        public static int solutionRealA(int M, int[] A)
        {
            int N = A.Length;
            int[] count = new int[M + 1];
            for (int i = 0; i <= M; i++)
                count[i] = 0;
            int maxOccurence = 1;
            int index = -1;
            for (int i = 0; i < N; i++)
            {
                if (count[A[i]] > 0)
                {
                    int tmp = count[A[i]];
                    if (tmp > maxOccurence)
                    {
                        maxOccurence = tmp;
                        index = i;
                    }
                    count[A[i]] = tmp + 1;
                }
                else
                {
                    count[A[i]] = 1;
                }
            }
            return A[index];
        }

        public static int solutionRealAMod(int M, int[] A)
        {
            int N = A.Length;
            int[] count = new int[M + 1];
            for (int i = 0; i <= M; i++)
                count[i] = 0;
            int maxOccurence = 0;
            int index = -1;
            for (int i = 0; i < N; i++)
            {
                if (count.Length > A[i])
                {
                    if (count[A[i]] > 0)
                    {
                        int tmp = count[A[i]];
                        if (tmp > maxOccurence)
                        {
                            maxOccurence = tmp;
                            index = i;
                        }
                        count[A[i]] = tmp + 1;
                    }
                    else
                    {
                        count[A[i]] = 1;
                    }
                }
            }
            return A[index];
        }

        public static int solutionTest(int[] A)
        {
            // write your code in C# 6.0 with .NET 4.5 (Mono)
            int smallest = 0;
            var lista = new List<int>(A);
            bool final_increment = true;
            lista = lista
                .OrderBy(c => c)
                .Distinct()
               .ToList();
            foreach (int item in lista)
            {
                if (item != ++smallest)
                {
                    final_increment = false;
                    break;
                }
            }
            if (final_increment) ++smallest;
            return smallest;
            //for (int i = 0; i < A.Length; i++)
            //{
            //    if(A[i] == smallest)
            //    {
            //        smallest++;
            //        i = -1;
            //    }
            //}
            //return smallest;
        }
    }
}

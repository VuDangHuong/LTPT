using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;
namespace TH1
{
    internal class TH1
    {
        private static int[] A;
        private static int[] Results;
        private static int N = 11;
        private static int k = 3;
    public static void Main(string[] args)
        {
            Random random = new Random();
            A = new int[N];
            for (int i = 0; i < N; i++)
            {
                A[i] = random.Next(100);
            }
            A[N - 1] = -1;
            Console.WriteLine("Array: " + string.Join(", ", A));
            Results = new int[k];
            Thread[] threads = new Thread[k];
            int segmentSize = N / k;

            for (int i = 0; i < k; i++)
            {
                int start = i * segmentSize;
                int end = (i == k - 1) ? N : start + segmentSize;
                int threadIndex = i;

                threads[i] = new Thread(() =>
                {
                    int min = FindMinInSegment(start, end);
                    Results[threadIndex] = min;
                    Console.WriteLine($"T{threadIndex + 1}: {min} : {DateTime.Now:HH:mm:ss.fff}");
                });

                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            int finalMin = Results[0];
            for (int i = 1; i < Results.Length; i++)
            {
                if (Results[i] < finalMin)
                {
                    finalMin = Results[i];
                }
            }

            Console.WriteLine($"Final Min: {finalMin}");
        }
        private static int FindMinInSegment(int start, int end)
        {
            int min = A[start];
            for (int i = start + 1; i < end; i++)
            {
                if (A[i] < min)
                {
                    min = A[i];
                }
            }
            return min;
        }
        }
    }

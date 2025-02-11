using System;
using System.Threading;

namespace lab1
{
    static class ParallelQuickSort
    {
        private static void QuickSort(int[] arr, int left, int right)
        {
            if (left >= right) return; // Base case of recursion

            var pivot = Partition(arr, left, right);

            var leftThread = new Thread(() => QuickSort(arr, left, pivot - 1));
            var rightThread = new Thread(() => QuickSort(arr, pivot + 1, right));

            leftThread.Start();
            rightThread.Start();

            leftThread.Join();
            rightThread.Join();
        }

        private static int Partition(int[] arr, int left, int right)
        {
            var pivot = arr[right]; // Choose the pivot element
            var i = left - 1;

            for (var j = left; j < right; j++)
            {
                if (arr[j] >= pivot) continue;
                i++;
                Swap(arr, i, j);
            }

            Swap(arr, i + 1, right);
            return i + 1; // Position of the pivot element
        }

        private static void Swap(int[] arr, int i, int j)
        {
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }

        private static void Main()
        {
            Console.Write("Enter the length of the array: ");
            int length = int.Parse(Console.ReadLine() ?? "10");

            int[] arr = new int[length];
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                arr[i] = random.Next(0, 101);
            }

            Console.WriteLine("Initial array: " + string.Join(", ", arr));

            QuickSort(arr, 0, arr.Length - 1);

            Console.WriteLine("Sorted array: " + string.Join(", ", arr));
        }
    }
}
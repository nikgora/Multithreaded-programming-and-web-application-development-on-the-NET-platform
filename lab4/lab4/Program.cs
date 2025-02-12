namespace lab4;

static class ParallelQuickSort
{
    private static void QuickSort(int[] arr, int left, int right)
    {
        if (left >= right) return; // Base case of recursion

        var pivot = Partition(arr, left, right);

        // Use Parallel.Invoke for parallel calls of the left and right partitions
        Parallel.Invoke(
            () => QuickSort(arr, left, pivot - 1),
            () => QuickSort(arr, pivot + 1, right)
        );
    }

    private static int Partition(int[] arr, int left, int right)
    {
        var pivot = arr[right]; // Pivot element
        var i = left - 1;

        for (var j = left; j < right; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                Swap(arr, i, j);
            }
        }

        Swap(arr, i + 1, right);
        return i + 1; // New pivot element position
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    private static void Main()
    {
        Console.Write("Enter the length of the array: ");
        var length = int.Parse(Console.ReadLine() ?? "10");

        var arr = new int[length];
        var random = new Random();
        for (var i = 0; i < length; i++)
        {
            arr[i] = random.Next(0, 101); // Filling with random numbers in the range 0-100
        }

        Console.WriteLine("Generated array: " + string.Join(", ", arr));

        QuickSort(arr, 0, arr.Length - 1);

        Console.WriteLine("Sorted array: " + string.Join(", ", arr));
    }
}
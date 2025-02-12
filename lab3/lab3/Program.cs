namespace lab3;

static class ParallelQuickSort
{
    private static readonly TaskFactory TaskFactory = new TaskFactory();

    private static void QuickSort(int[] arr, int left, int right)
    {
        if (left >= right) return; // Base case of recursion

        var pivot = Partition(arr, left, right);

        // Using TaskFactory to create and start tasks
        var leftTask = TaskFactory.StartNew(() => QuickSort(arr, left, pivot - 1));
        var rightTask = TaskFactory.StartNew(() => QuickSort(arr, pivot + 1, right));

        Task.WaitAll(leftTask, rightTask); // Wait for completion of all threads
    }

    private static int Partition(int[] arr, int left, int right)
    {
        var pivot = arr[right]; // Pivot element
        var i = left - 1;

        for (var j = left; j < right; j++)
        {
            if (arr[j] >= pivot) continue;
            i++;
            Swap(arr, i, j);
        }

        Swap(arr, i + 1, right);
        return i + 1; // New position of pivot element
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
            arr[i] = random.Next(0, 101); // Random number between 0 and 100
        }
        Console.WriteLine("Initial array: " + string.Join(", ", arr));

        QuickSort(arr, 0, arr.Length - 1);

        Console.WriteLine("Sorted array: " + string.Join(", ", arr));
    }
}
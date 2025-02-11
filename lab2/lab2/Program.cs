namespace lab2;

static class ParallelQuickSort
{
    private static async Task QuickSort(int[] arr, int left, int right)
    {

        if (left >= right) return; // Base case of recursion

        var pivot = Partition(arr, left, right);

        // Recursively sort the left and right parts in parallel
        var leftTask = QuickSort(arr, left, pivot - 1);
        var rightTask = QuickSort(arr, pivot + 1, right);

        // Wait for both sides to finish
        await Task.WhenAll(leftTask, rightTask);
    }
    
    private static int Partition(int[] arr, int left, int right)
    {
        var pivotValue = arr[right];  // Choose pivot from the end
        var i = left - 1;

        for (var j = left; j < right; j++)
        {
            if (arr[j] >= pivotValue) continue;
            i++;
            Swap(arr, i, j);
        }

        Swap(arr, i + 1, right);
        return i + 1; // New pivot index
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    private static async Task Main()
    {
        Console.Write("Enter the length of the array: ");
        var length = int.Parse(Console.ReadLine() ?? "10");

        var arr = new int[length];
        var random = new Random();
        for (var i = 0; i < length; i++)
        {
            arr[i] = random.Next(0, 101);
        }

        Console.WriteLine("Initial array: " + string.Join(", ", arr));

        await QuickSort(arr, 0, arr.Length - 1);

        Console.WriteLine("Sorted array: " + string.Join(", ", arr));
    }
}
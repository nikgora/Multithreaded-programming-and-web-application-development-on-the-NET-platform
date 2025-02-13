using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async context =>
{
    var html = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>Bubble Sort Web App</title>
</head>
<body>
    <h2>Bubble Sort Web Application</h2>
    <form action=""/sort"" method=""post"">
        <div>
            <label for=""numbers"">Enter numbers (comma-separated):</label>
            <input type=""text"" id=""numbers"" name=""numbers"" required />
        </div>
        <div>
            <button type=""submit"">Sort Numbers</button>
        </div>
    </form>
</body>
</html>";

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

app.MapPost("/sort", async context =>
{
    var form = await context.Request.ReadFormAsync();
    string numbersInput = form["numbers"];

    // Validate and parse input numbers as floats
    float[] numbers;
    try
    {
        numbers = numbersInput.Split(',')
                              .Select(n => float.Parse(n.Trim(), CultureInfo.InvariantCulture))
                              .ToArray();
    }
    catch
    {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("<p>Invalid input. Please enter numbers separated by commas.</p>");
        return;
    }

    // Perform Bubble Sort
    BubbleSort(numbers);
    
    var formattedNumbers = string.Join(", ", numbers.Select(n => n.ToString(CultureInfo.InvariantCulture)));

    // Generate response HTML
    var resultHtml = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>Sorted Numbers</title>
</head>
<body>
    <h2>Sorted Numbers</h2>
    <p>Original Input: {numbersInput}</p>
    <p>Sorted Output: <strong>{string.Join(", ", formattedNumbers)}</strong></p>
    <a href=""/"">Back to Input</a>
</body>
</html>";

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(resultHtml);
});

app.Run();
return;

void BubbleSort(float[] arr)
{
    var n = arr.Length;
    for (var i = 0; i < n - 1; i++)
    {
        var swapped = false;
        for (var j = 0; j < n - i - 1; j++)
        {
            if (
                arr[j] <= arr[j + 1]) continue;
            (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]); // Swap using tuple
            swapped = true;
        }
        if (!swapped) break; // Stop if already sorted
    }
}

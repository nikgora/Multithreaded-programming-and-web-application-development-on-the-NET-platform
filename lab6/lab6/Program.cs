using System.Globalization;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Enable default file mapping and static file serving from wwwroot.
app.UseDefaultFiles(); // Looks for index.html by default
app.UseStaticFiles();

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

    // Prepare the sorted numbers as a comma-separated string
    var sortedNumbers = string.Join(", ", numbers.Select(n => n.ToString(CultureInfo.InvariantCulture)));

    // Load the external HTML file for the result page.
    var resultHtmlPath = Path.Combine(app.Environment.WebRootPath, "result.html");
    var resultHtml = await File.ReadAllTextAsync(resultHtmlPath);

    // Replace placeholders in the HTML template with dynamic values.
    resultHtml = resultHtml.Replace("{{INPUT}}", numbersInput)
                           .Replace("{{SORTED}}", sortedNumbers);

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(resultHtml);
});

void BubbleSort(float[] arr)
{
    int n = arr.Length;
    for (int i = 0; i < n - 1; i++)
    {
        bool swapped = false;
        for (int j = 0; j < n - i - 1; j++)
        {
            if (arr[j] > arr[j + 1])
            {
                (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]); // Swap using tuple syntax
                swapped = true;
            }
        }
        if (!swapped)
            break; // Stop if the array is already sorted.
    }
}

app.Run();

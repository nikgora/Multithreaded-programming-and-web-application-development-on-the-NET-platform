using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async context =>
{
    var html = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>Trapezoidal Integral Calculator</title>
</head>
<body>
    <h2>Trapezoidal Integral Calculator Log10(x) * x + 2 * x + 1</h2>
    <form action=""/calculate"" method=""post"">
        <div>
            <label for=""A"">Lower Limit (A):</label>
            <input type=""number"" step=""0.01""  id=""A"" name=""A"" required />
        </div>
        <div>
            <label for=""B"">Upper Limit (B):</label>
            <input type=""number"" step=""any"" id=""B"" name=""B"" required />
        </div>
        <div>
            <label for=""N"">Number of Subintervals (N):</label>
            <input type=""number"" id=""N"" name=""N"" required min=""1"" />
        </div>

        <div>
            <button type=""submit"">Calculate Integral</button>
        </div>
    </form>
</body>
</html>";
    
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

app.MapPost("/calculate", async context =>
{
    var form = await context.Request.ReadFormAsync();

    if (form.Count == 0)
    {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("<p>No input values found. Please go back and try again.</p>");
        return;
    }

    string aString = form["A"];
    string bString = form["B"];
    string nString = form["N"];

    if (!double.TryParse(aString, NumberStyles.Any, CultureInfo.InvariantCulture, out double A) ||
        !double.TryParse(bString, NumberStyles.Any, CultureInfo.InvariantCulture, out double B) ||
        !int.TryParse(nString, out int n) || n < 1)
    {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("<p>Invalid input values. Please go back and try again.</p>");
        return;
    }

    double h = (B - A) / n;
    double sum = (Function(A) + Function(B)) / 2.0;
    for (int i = 1; i < n; i++)
    {
        double x = A + i * h;
        sum += Function(x);
    }
    double result = sum * h;

    var resultHtml = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>Calculation Result</title>
</head>
<body>
    <h2>Calculation Result</h2>
    <p>
        The integral of f(x) =  Log10(x) * x + 2 * x + 1 from {A} to {B} using {n} subintervals is:
        <strong>{result}</strong>
    </p>
    <a href=""/"">Back to Calculator</a>
</body>
</html>";
    
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(resultHtml);
});

double Function(double x)
{
    return Math.Log10(x) * x + 2 * x + 1;
}

app.Run();

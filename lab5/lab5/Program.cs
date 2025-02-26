using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Enable serving default files (like index.html) and static files from wwwroot.
app.UseDefaultFiles(); // Automatically looks for index.html
app.UseStaticFiles();

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

    // Load the external HTML file for the result page.
    var resultHtmlPath = Path.Combine(app.Environment.WebRootPath, "result.html");
    var resultHtml = await File.ReadAllTextAsync(resultHtmlPath);

    // Replace placeholders in the HTML template with dynamic values.
    resultHtml = resultHtml.Replace("{{A}}", A.ToString())
                           .Replace("{{B}}", B.ToString())
                           .Replace("{{N}}", n.ToString())
                           .Replace("{{RESULT}}", result.ToString());

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(resultHtml);
});

double Function(double x)
{
    // Note: Math.Log10(x) is only defined for x > 0.
    return Math.Log10(x) * x + 2 * x + 1;
}

app.Run();

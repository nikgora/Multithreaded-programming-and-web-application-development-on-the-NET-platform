using Microsoft.AspNetCore.Mvc;

namespace lab7.Controllers
{
    public class IntegrationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(IntegrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            model.Result = CalculateTrapezoidalIntegral(model);
            return View("Result", model);
        }

        private double CalculateTrapezoidalIntegral(IntegrationModel model)
        {
            double h = (model.UpperLimit - model.LowerLimit) / model.SubIntervals;
            double sum = (Function(model.LowerLimit, model.FunctionType) + Function(model.UpperLimit, model.FunctionType)) / 2.0;

            for (int i = 1; i < model.SubIntervals; i++)
            {
                double x = model.LowerLimit + i * h;
                sum += Function(x, model.FunctionType);
            }

            return sum * h;
        }

        private double Function(double x, string functionType)
        {
            return functionType switch
            {
                "x2" => x * x,
                "sinx" => Math.Sin(x),
                "lnx" => x > 0 ? Math.Log(x) : 0, // Prevent Log(0) error
                _ => x * x
            };
        }
    }
}
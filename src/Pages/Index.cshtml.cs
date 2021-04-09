using Davepermen.Website.Fitness.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

namespace Davepermen.Website.Fitness.Pages
{
    public class IndexModel : PageModel
    {
        TrainingData trainingData;

        public int Pushups => trainingData.Sum;

        public int PushupsToday => trainingData.AmountPerDay[DateTime.Today.DayOfYear - 1]; // DayOfYear is not starting at 0

        public int YearGoal => trainingData.Goal;

        public int Year => trainingData.Year;

        public void OnGet([FromQuery] int? year, [FromQuery] string user, [FromQuery] string training)
        {
            training ??= "pushups";
            user ??= (User.Identity.IsAuthenticated ? User.Identity.Name : "davepermen");

            year ??= DateTime.Now.Year;

            trainingData = new TrainingData(user, training, year.Value);
        }

        public string DailyPushupsGraph => string.Join(" ", trainingData.AmountPerDay.Select((value, index) => $"{index}, {value * .25f}")); // format for svg polyline
        public string DailyPushupsGraphBackground => $"0, 0 {DailyPushupsGraph} 364, 0";
        public string SummedPushupsGraph => string.Join(" ", trainingData.AccumulatedEveryDay.Select((value, index) => $"{index}, {value / (Math.Max(YearGoal, trainingData.Sum) / 100f)}")); // format for svg polyline
        public string SummedPushupsGraphBackground => $"0, 0 {SummedPushupsGraph} 364, 0";
    }
}
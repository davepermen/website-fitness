using Davepermen.Website.Fitness.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Davepermen.Website.Fitness.API;

[Route("[controller]")]
[ApiController]
public class FitnessController : Controller
{
    readonly int year = DateTime.Now.Year;

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromForm] int amount, [FromForm] string training, [FromForm] string redirectto)
    {
        if ((User?.Identity?.IsAuthenticated ?? false) && User?.Identity?.Name != null)
        {
            var user = User.Identity.Name;

            var trainingData = new TrainingData(user, training, year);
            await trainingData.AddAsync(amount);

            return Redirect(redirectto);
        }
        else
        {
            return Unauthorized();
        }
    }
}

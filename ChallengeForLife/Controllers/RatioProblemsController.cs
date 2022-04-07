using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeForLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatioProblemsController : ControllerBase
    {
        private readonly DataContext context;

        public RatioProblemsController(DataContext context)
        {
            this.context = context;
        }
        private static string? Proportions(string arr)
        {
            try
            {
                List<int> list = arr.TrimEnd().Split(' ').ToList().Select(arrTemp => Convert.ToInt32(arrTemp)).ToList();

                decimal positives = 0, negatives = 0, zeros = 0, length = list.Count;

                for (int i = 0; i < length; i++)
                {
                    if (list[i] < 0) negatives++;
                    else if (list[i] > 0) positives++;
                    else zeros++;
                }

                string result = $"positives: " + (positives / length).ToString("N6") + "; negatives: " + (negatives / length).ToString("N6") + "; zeros: " + (zeros / length).ToString("N6") + ".";
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]  string arr)
        {
            try
            {
                RatioProblems problems = new();
                if (problems.Id == System.Guid.Empty || string.IsNullOrEmpty(arr))
                {
                    return BadRequest();
                }

                problems.Input = arr;
                problems.Output = Proportions(arr);
                context.RatioProblems.Add(problems);
                context.SaveChanges();
                return Ok(problems);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:Guid?}")]
        public IActionResult Get(Guid? id)
        {
            var guidIsEmpty = id == null;
            if (guidIsEmpty)
            {
                try
                {
                    return Ok(context.RatioProblems.ToList().OrderByDescending(x => x.Date));
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            try
            {
                var sum = context.RatioProblems.Find(id);
                if (sum == null)
                {
                    return NoContent();
                }
                return Ok(sum);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
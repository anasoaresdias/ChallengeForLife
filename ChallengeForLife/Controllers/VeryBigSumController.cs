using Microsoft.AspNetCore.Mvc;

namespace ChallengeForLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeryBigSumController : ControllerBase
    {
        private readonly DataContext context;

        public VeryBigSumController(DataContext context)
        {
            this.context = context;
        }

        private static long Summary(string sum)
        {
            List<long> ar = sum.TrimEnd().Split(' ').ToList().Select(arTemp => Convert.ToInt64(arTemp)).ToList();
            long result = ar.Sum();
            return result;
        }

        [HttpGet("{id:Guid?}")]
        public IActionResult Get(Guid? id)
        {
            var guidIsEmpty = id == null;
            if (guidIsEmpty)
            {
                try
                {
                    return Ok(context.VeryBigSum.ToList().OrderByDescending(x => x.Date));
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            try
            {
                var sum = context.VeryBigSum.Find(id);
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

        [HttpPost]
        public IActionResult Post([FromBody] string sum)
        {
            try
            {
                VeryBigSum veryBigSum = new();
                if (veryBigSum.Id == System.Guid.Empty || string.IsNullOrEmpty(sum))
                {
                    return BadRequest();
                }

                veryBigSum.Input = sum;
                veryBigSum.Output = Summary(sum).ToString();

                context.VeryBigSum.Add(veryBigSum);
                context.SaveChanges();

                return Ok(veryBigSum);
            }
            catch (Exception)
            {
                return BadRequest("Error in input.");
            }
            
        }
    }
}

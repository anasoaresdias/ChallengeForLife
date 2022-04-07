using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChallengeForLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagonalSumController : ControllerBase
    {
        private readonly DataContext context;

        public DiagonalSumController(DataContext context)
        {
            this.context = context;
        }
        
        private static int Diference(List<List<int>> arr)
        {
            int leftdiagonal = 0, rightdiagonal = 0;
            for (int i = 0; i < arr.Count; i++)
            {
                leftdiagonal += arr[i][0 + i];
                rightdiagonal += arr[i][arr.Count - i - 1];
            }
            return Math.Abs(rightdiagonal - leftdiagonal);
        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            DiagonalSum sum = new();
            List<string> list = new();
            List<List<int>> arr = new();

            if (sum.Id == System.Guid.Empty)
            {
                return BadRequest();
            }
            try
            {
                list = input.TrimEnd().Split(',').ToList();
                list.ForEach(x => arr.Add(x.TrimEnd().Split(' ').ToList().Select(arrTemp => Convert.ToInt32(arrTemp)).ToList()));

                sum.Input = input;
                sum.Output = Diference(arr).ToString();

                context.DiagonalSum.Add(sum);
                context.SaveChanges();

                return Ok(sum);
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
                    return Ok(context.DiagonalSum.ToList().OrderByDescending(x => x.Date));
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            try
            {
                var sum = context.DiagonalSum.Find(id);
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
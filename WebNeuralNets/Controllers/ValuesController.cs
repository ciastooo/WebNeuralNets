using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebNeuralNets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ValuesController()
        {
        }

        // GET api/values
        /// <summary>
        /// Returns test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new string[] { "value1", "value2" });
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            return Ok($"{id} value");
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return Created("", value);
        }

        // PUT api/values/5
        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}

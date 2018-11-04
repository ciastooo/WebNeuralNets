using System.Web.Http;
using System.Net;

namespace WebNeuralNets.Controllers
{

    [RoutePrefix("api/[controller]")]
    public class ValuesController : ApiController
    {

        public ValuesController()
        {
        }

        // GET api/values
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new string[] { "value1", "value2" });
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok($"{id} value");
        }

        // POST api/values
        [HttpPost]
        public IHttpActionResult Post([FromBody] string value)
        {
            return Created("", value);
        }

        // PUT api/values/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HardwareInventoryManager.Controllers.Api
{
    public class AliveController : ApiController
    {
        // GET: api/Alive
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Alive/5
        public IHttpActionResult Get(int id)
        {
            return Ok(id);
        }

        // POST: api/Alive
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Alive/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Alive/5
        public void Delete(int id)
        {
        }
    }
}

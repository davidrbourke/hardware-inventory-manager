using AutoMapper;
using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace HardwareInventoryManager.Controllers.Api
{
    public class QuoteRequestsController : ApiController
    {
        // GET: api/QuoteRequests
        public IEnumerable<QuoteRequestViewModel> Get()
        {
            IList<QuoteRequest> quoteRequests = new List<QuoteRequest>();

            for(int i = 0; i < 100; i++)
            {
                quoteRequests.Add(
                new QuoteRequest
                {
                    QuoteRequestId = i,
                    DateRequired = DateTime.Now,
                    SpecificationDetails = "A new computer",
                    Quantity = i * 10
                });
            }

            Mapper.CreateMap<QuoteRequest, QuoteRequestViewModel>();
            var l = Mapper.Map<IList<QuoteRequest>, IList<QuoteRequestViewModel>>(quoteRequests);

            return l;
        }

        // GET: api/AssetsApi/5
        [ResponseType(typeof(QuoteRequestViewModel))]
        public IHttpActionResult Get(int id)
        {
            CustomApplicationDbContext db = new CustomApplicationDbContext();
            IQueryable<Lookup> itemTypes = db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Category.ToString());

            var quoteRequestViewModel = new QuoteRequestViewModel
            {
                ItemTypes = itemTypes
            };
            return Ok(quoteRequestViewModel);
        }

        // POST: api/QuoteRequests
        public void Post([FromBody]QuoteRequestViewModel value)
        {
        }

        // PUT: api/QuoteRequests/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/QuoteRequests/5
        public void Delete(int id)
        {
        }
    }
}

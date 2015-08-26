using AutoMapper;
using HardwareInventoryManager.Filters;
using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Helpers.User;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Services.Quotes;
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
    [Authorize]
    public class QuoteRequestsController : ApiController
    {
        // GET: api/QuoteRequests
        public IEnumerable<QuoteRequestViewModel> Get()
        {
            IList<QuoteRequest> quoteRequests = new List<QuoteRequest>();
            IRepository<QuoteRequest> quoteRepository = new Repository<QuoteRequest>();
            quoteRepository.SetCurrentUserByUsername(User.Identity.Name);
            QuoteRequestService quoteRequestService = new QuoteRequestService(quoteRepository);
            quoteRequests = quoteRequestService.GetAllQuote().ToList();
            Mapper.CreateMap<QuoteRequest, QuoteRequestViewModel>();
            return Mapper.Map<IList<QuoteRequest>, IList<QuoteRequestViewModel>>(quoteRequests);
        }

        // GET: api/AssetsApi/5
        [ResponseType(typeof(QuoteRequestViewModel))]
        public IHttpActionResult Get(int id)
        {
            CustomApplicationDbContext db = new CustomApplicationDbContext();
            IQueryable<Lookup> itemTypes = db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Category.ToString());
            IQueryable<Tenant> tenants = db.Tenants.Where(t => t.Users.Where(u => u.UserName == User.Identity.Name).Any());

            Mapper.CreateMap<Tenant, TenantViewModel>();
            var listOfTenantViewModel = Mapper.Map<IEnumerable<Tenant>, IEnumerable<TenantViewModel>>(tenants.ToList());

            if (id == -1)
            {
                QuoteRequestViewModel quoteRequestTemplate = new QuoteRequestViewModel();
                quoteRequestTemplate.ItemTypes = itemTypes;
                quoteRequestTemplate.Tenants = listOfTenantViewModel;
                return Ok(quoteRequestTemplate);
            }
            
            IRepository<QuoteRequest> quoteRepository = new Repository<QuoteRequest>(db);
            quoteRepository.SetCurrentUserByUsername(User.Identity.Name);
            QuoteRequestService quoteService = new QuoteRequestService(quoteRepository);
            QuoteRequest quoteRequest = quoteService.GetSingleQuote(id);
            Mapper.CreateMap<QuoteRequest, QuoteRequestViewModel>();
            QuoteRequestViewModel quoteRequestViewModel = Mapper.Map<QuoteRequestViewModel>(quoteRequest);
            quoteRequestViewModel.ItemTypes = itemTypes.ToList();
            quoteRequestViewModel.SelectedItemType = quoteRequest.Category;
            quoteRequestViewModel.Tenants = listOfTenantViewModel;
            return Ok(quoteRequestViewModel);
            //return Json<QuoteRequestViewModel>(quoteRequestViewModel);
        }

        // POST: api/QuoteRequests
        [ResponseType(typeof(QuoteRequest))]
        public IHttpActionResult Post([FromBody]QuoteRequestViewModel value)
        {
            if(ModelState.IsValid)
            {
                IRepository<QuoteRequest> quoteRepository = new Repository<QuoteRequest>();
                Mapper.CreateMap<QuoteRequestViewModel, QuoteRequest>();
                QuoteRequest quoteRequestToCreate = Mapper.Map<QuoteRequest>(value);
                quoteRequestToCreate.TenantId = value.SelectedTenant.TenantId;
                quoteRequestToCreate.CategoryId = value.SelectedItemType.LookupId;
                quoteRepository.SetCurrentUserByUsername(User.Identity.Name);
                QuoteRequestService quoteRequestService = new QuoteRequestService(quoteRepository);
                quoteRequestService.SaveQuoteRequest(quoteRequestToCreate);
            }
            else
            {

                //IEnumerable<string> errors = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }
            return Ok("success");
            //return CreatedAtRoute("DefaultApi", "success",  value);
        }

        // PUT: api/QuoteRequests/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/QuoteRequests/5
        public IHttpActionResult Delete(int id)
        {
            IRepository<QuoteRequest> quoteRepository = new Repository<QuoteRequest>();
            quoteRepository.SetCurrentUserByUsername(User.Identity.Name);
            QuoteRequestService quoteRequestService = new QuoteRequestService(quoteRepository);
            quoteRequestService.DeleteQuoteRequest(id);
            return Ok("success");
        }
    }
}

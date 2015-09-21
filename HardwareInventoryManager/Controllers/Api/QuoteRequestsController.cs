using AutoMapper;
using HardwareInventoryManager.Filters;
using HardwareInventoryManager.Services;
using HardwareInventoryManager.Services.User;
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
            IList<QuoteRequest> quoteRequests = QuoteRequestService.GetAllQuotes().OrderByDescending(x => x.QuoteRequestId)
                .ToList();

            QuoteRequest recentQuoteRequest = quoteRequests.Where(x => x.CreatedDate > DateTime.Now.AddSeconds(-10)).FirstOrDefault();
            if(recentQuoteRequest != null)
                recentQuoteRequest.NewQuoteRequest = true;

            Mapper.CreateMap<QuoteRequest, QuoteRequestViewModel>();
            return Mapper.Map<IList<QuoteRequest>, IList<QuoteRequestViewModel>>(quoteRequests);

        }

        // GET: api/AssetsApi/5
        [ResponseType(typeof(QuoteRequestViewModel))]
        public IHttpActionResult Get(int id)
        {
            CustomApplicationDbContext db = new CustomApplicationDbContext();
            IQueryable<Lookup> itemTypes = db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Category.ToString());
            IQueryable<Lookup> quoteRequestStatusTypes = db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.QuoteRequestStatus.ToString());
            IQueryable<Tenant> tenants = db.Tenants.Where(t => t.Users.Where(u => u.UserName == User.Identity.Name).Any());
            Mapper.CreateMap<Tenant, TenantViewModel>();
            var listOfTenantViewModel = Mapper.Map<IEnumerable<Tenant>, IEnumerable<TenantViewModel>>(tenants.ToList());
            if (id == -1)
            {
                QuoteRequestViewModel quoteRequestTemplate = new QuoteRequestViewModel();
                quoteRequestTemplate.ItemTypes = itemTypes;
                quoteRequestTemplate.Tenants = listOfTenantViewModel;
                quoteRequestTemplate.QuoteRequestStatuses = quoteRequestStatusTypes;
                quoteRequestTemplate.SelectedQuoteRequestStatus = quoteRequestStatusTypes.FirstOrDefault(x => x.Description == "Pending");
                return Ok(quoteRequestTemplate);
            }
            QuoteRequest quoteRequest = QuoteRequestService.GetSingleQuote(id);
            Mapper.CreateMap<QuoteRequest, QuoteRequestViewModel>();
            QuoteRequestViewModel quoteRequestViewModel = Mapper.Map<QuoteRequestViewModel>(quoteRequest);
            quoteRequestViewModel.ItemTypes = itemTypes.ToList();
            quoteRequestViewModel.QuoteRequestStatuses = quoteRequestStatusTypes.ToList();
            quoteRequestViewModel.SelectedItemType = quoteRequest.Category;
            quoteRequestViewModel.SelectedQuoteRequestStatus = quoteRequest.QuoteRequestStatus;
            quoteRequestViewModel.Tenants = listOfTenantViewModel;
            quoteRequestViewModel.CanChangeStatus = User.IsInRole("Admin") ? true : false;
            return Ok(quoteRequestViewModel);
            //return Json<QuoteRequestViewModel>(quoteRequestViewModel);
        }

        // POST: api/QuoteRequests
        [ResponseType(typeof(QuoteRequest))]
        public IHttpActionResult Post([FromBody]QuoteRequestViewModel value)
        {
            if(ModelState.IsValid)
            {
                Mapper.CreateMap<QuoteRequestViewModel, QuoteRequest>();
                QuoteRequest quoteRequestToCreate = Mapper.Map<QuoteRequest>(value);
                quoteRequestToCreate.TenantId = value.SelectedTenant.TenantId;
                quoteRequestToCreate.CategoryId = value.SelectedItemType.LookupId;
                quoteRequestToCreate.QuoteRequestStatusId = value.SelectedQuoteRequestStatus.LookupId;
                QuoteRequestService.SaveQuoteRequest(quoteRequestToCreate);
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
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            QuoteRequestService.DeleteQuoteRequest(id);
            return Ok("success");
        }

        private QuoteRequestService _quoteRequestService;
        public QuoteRequestService QuoteRequestService
        {
            get
            {
                if(_quoteRequestService == null)
                {
                    _quoteRequestService = new QuoteRequestService(User.Identity.Name);
                }
                return _quoteRequestService;
            }
            set
            {
                _quoteRequestService = value;
            }
        }
        
        //[AllowAnonymous]
        //public IHttpActionResult Alive()
        //{
        //    return Ok();
        //}
    
    }
}

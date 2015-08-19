using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HardwareInventoryManager;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Filters;

namespace HardwareInventoryManager.Controllers
{
    [CustomAuthorize]
    public class QuoteRequestsController : Controller
    {
        private CustomApplicationDbContext db = new CustomApplicationDbContext();

        // GET: QuoteRequests
        public ActionResult Index()
        {
            var quoteRequests = db.QuoteRequests.Include(q => q.QuoteResponse);
            return View(quoteRequests.ToList());
        }

        // GET: QuoteRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuoteRequest quoteRequest = db.QuoteRequests.Find(id);
            if (quoteRequest == null)
            {
                return HttpNotFound();
            }
            return View(quoteRequest);
        }

        // GET: QuoteRequests/Create
        public ActionResult Create()
        {
            ViewBag.QuoteRequestId = new SelectList(db.QuoteResponses, "QuoteReposonseId", "Notes");
            return View();
        }

        // POST: QuoteRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuoteRequestId,DateRequired,Quantity,SpecificationDetails,QuoteResponseId")] QuoteRequest quoteRequest)
        {
            if (ModelState.IsValid)
            {
                db.QuoteRequests.Add(quoteRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuoteRequestId = new SelectList(db.QuoteResponses, "QuoteReposonseId", "Notes", quoteRequest.QuoteRequestId);
            return View(quoteRequest);
        }

        // GET: QuoteRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuoteRequest quoteRequest = db.QuoteRequests.Find(id);
            if (quoteRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuoteRequestId = new SelectList(db.QuoteResponses, "QuoteReposonseId", "Notes", quoteRequest.QuoteRequestId);
            return View(quoteRequest);
        }

        // POST: QuoteRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuoteRequestId,DateRequired,Quantity,SpecificationDetails,QuoteResponseId")] QuoteRequest quoteRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quoteRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuoteRequestId = new SelectList(db.QuoteResponses, "QuoteReposonseId", "Notes", quoteRequest.QuoteRequestId);
            return View(quoteRequest);
        }

        // GET: QuoteRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuoteRequest quoteRequest = db.QuoteRequests.Find(id);
            if (quoteRequest == null)
            {
                return HttpNotFound();
            }
            return View(quoteRequest);
        }

        // POST: QuoteRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuoteRequest quoteRequest = db.QuoteRequests.Find(id);
            db.QuoteRequests.Remove(quoteRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

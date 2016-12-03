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
    public class QuoteResponsesController : Controller
    {
        private CustomApplicationDbContext db = new CustomApplicationDbContext();

        // GET: QuoteResponses
        public ActionResult Index()
        {
            var quoteResponses = db.QuoteResponses.Include(q => q.QuoteRequest);
            return View(quoteResponses.ToList());
        }

        // GET: QuoteResponses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuoteResponse quoteResponse = db.QuoteResponses.Find(id);
            if (quoteResponse == null)
            {
                return HttpNotFound();
            }
            return View(quoteResponse);
        }

        // GET: QuoteResponses/Create
        public ActionResult Create()
        {
            ViewBag.QuoteReposonseId = new SelectList(db.QuoteRequests, "QuoteRequestId", "SpecificationDetails");
            return View();
        }

        // POST: QuoteResponses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuoteReposonseId,QuoteCostPerItem,QuoteCostTotal,Notes,QuoteRequestId")] QuoteResponse quoteResponse)
        {
            if (ModelState.IsValid)
            {
                db.QuoteResponses.Add(quoteResponse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuoteReposonseId = new SelectList(db.QuoteRequests, "QuoteRequestId", "SpecificationDetails", quoteResponse.QuoteReposonseId);
            return View(quoteResponse);
        }

        // GET: QuoteResponses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuoteResponse quoteResponse = db.QuoteResponses.Find(id);
            if (quoteResponse == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuoteReposonseId = new SelectList(db.QuoteRequests, "QuoteRequestId", "SpecificationDetails", quoteResponse.QuoteReposonseId);
            return View(quoteResponse);
        }

        // POST: QuoteResponses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuoteReposonseId,QuoteCostPerItem,QuoteCostTotal,Notes,QuoteRequestId")] QuoteResponse quoteResponse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quoteResponse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuoteReposonseId = new SelectList(db.QuoteRequests, "QuoteRequestId", "SpecificationDetails", quoteResponse.QuoteReposonseId);
            return View(quoteResponse);
        }

        // GET: QuoteResponses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuoteResponse quoteResponse = db.QuoteResponses.Find(id);
            if (quoteResponse == null)
            {
                return HttpNotFound();
            }
            return View(quoteResponse);
        }

        // POST: QuoteResponses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuoteResponse quoteResponse = db.QuoteResponses.Find(id);
            db.QuoteResponses.Remove(quoteResponse);
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

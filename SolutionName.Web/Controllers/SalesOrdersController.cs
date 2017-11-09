using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SolutionName.DataLayer;
using SolutionName.Model;
using SolutionName.Web.ViewModel;

namespace SolutionName.Web.Controllers
{
    public class SalesOrdersController : Controller
    {
        private SalesContext _salesContext;


        public SalesOrdersController()
        {
            _salesContext = new SalesContext();
        }

        // GET: SalesOrders
        public ActionResult Index()
        {
            return View(_salesContext.SalesOrders.ToList());
        }

        // GET: SalesOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);        
            if (salesOrder == null)
            {
                return HttpNotFound();
            }

            SalesOrderViewModel salesOrderViewModel =
                Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
               
            salesOrderViewModel.MessageToClient = "I originated from the viewmodel.";

            return View(salesOrderViewModel);
        }

        // GET: SalesOrders/Create
        public ActionResult Create()
        {
            SalesOrderViewModel salesOrderViewModel = new SalesOrderViewModel();
            salesOrderViewModel.ObjectState = ObjectState.Added;
            return View(salesOrderViewModel);
        }

        // POST: SalesOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalesOrderId,CustomerName,PONumber")] SalesOrder salesOrder)
        {
            if (ModelState.IsValid)
            {
                _salesContext.SalesOrders.Add(salesOrder);
                _salesContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salesOrder);
        }

        // GET: SalesOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }

            SalesOrderViewModel salesOrderViewModel = Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
            salesOrderViewModel.MessageToClient = string.Format("The original value of customer name is {0}", salesOrderViewModel.CustomerName);

            return View(salesOrderViewModel);
        }

        // POST: SalesOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SalesOrderId,CustomerName,PONumber")] SalesOrder salesOrder)
        {
            if (ModelState.IsValid)
            {
                _salesContext.Entry(salesOrder).State = EntityState.Modified;
                _salesContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesOrder);
        }

        // GET: SalesOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }

            SalesOrderViewModel salesOrderViewModel = Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
            salesOrderViewModel.MessageToClient = string.Format("You are about to delete this sales order.");
            salesOrderViewModel.ObjectState = ObjectState.Deleted;

            return View(salesOrderViewModel);
        }

        // POST: SalesOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);
            _salesContext.SalesOrders.Remove(salesOrder);
            _salesContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _salesContext.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult Save(SalesOrderViewModel salesOrderViewModel)
        {
            SalesOrder salesOrder = Helpers.CreateSalesOrderFromSalesOrderViewModel(salesOrderViewModel);           

            _salesContext.SalesOrders.Attach(salesOrder);

            if (salesOrder.ObjectState == ObjectState.Deleted)
            {
                foreach(SalesOrderItemViewModel salesOrderItemViewModel in salesOrderViewModel.SalesOrderItems)
                {
                    SalesOrderItem salesOrderItem = _salesContext.SalesOrderItems.Find(salesOrderItemViewModel.SalesOrderItemId);
                    if (salesOrderItem != null)
                        salesOrderItem.ObjectState = ObjectState.Deleted;
                }
            }
            else
            {
                foreach (int salesOrderItemId in salesOrderViewModel.SalesOrderItemsToDelete)
                {
                    SalesOrderItem salesOrderItem = _salesContext.SalesOrderItems.Find(salesOrderItemId);
                    if (salesOrderItem != null)
                        salesOrderItem.ObjectState = ObjectState.Deleted;
                }
            }

            //_salesContext.ChangeTracker.Entries<IObjectWithState>().Single().State = SolutionName.DataLayer.Helpers.ConvertState(salesOrder.ObjectState);
            _salesContext.ApplyStateChanges();
            _salesContext.SaveChanges();

            if (salesOrder.ObjectState == ObjectState.Deleted)
                return Json(new { newLocation ="/SalesOrders/Index/"});

            string messageToClient = Helpers.GetMessageToClient(salesOrderViewModel.ObjectState, salesOrder.CustomerName);
            salesOrderViewModel = Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
            salesOrderViewModel.MessageToClient = messageToClient;

            return Json(new { salesOrderViewModel });

        }
    }
}

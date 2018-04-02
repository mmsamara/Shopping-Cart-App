using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MuhammadShoppingCart.Models;
using Microsoft.AspNet.Identity;

namespace MuhammadShoppingCart.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            //You are going out and getting the Orders table, and including information about the customer. You can only do this b/v of the Customer navigation property in the Order model.
            //var orders = db.Orders.Include(o => o.Customer);
            var userId = User.Identity.GetUserId();
            var orderHistory = new List<Order>();

            if (userId == null)
            {
                orderHistory = db.Orders.ToList();
            }
            else
            {
                orderHistory = db.Orders.Where(o => o.CustomerId == userId).ToList();
            }
            return View(orderHistory);
        }
        // GET: Orders
        public ActionResult Complete(int? id)
        {
            if(id != null)
            {
                //"You must be loggen in, & I'm gonna grab the id of whoever's logged in & assign that to the variable userid
                var userid = User.Identity.GetUserId();

                var completeOrder = db.Orders.Find(id);

                //Here we're using the Navigation property of OrderDetails to get a reference to this Orders details. See Models > Order > Line 26
                var orderdetail = completeOrder.OrderDetails.ToList();

                //Going out to ShoppingCart table & locating the record who's CustomerId is equal to userId, using Where clause. Then assign value to shoppingcarts
                //"s" in the lambda statement is referring to the shoppingcarts table. reads like: "s goes to s.CustomerId equals userid"
                var shoppingcarts = db.ShoppingCarts.Where(s => s.CustomerId == userid);

                //Now that we completed the Order we need to clear out the ShoppingCart
                if (shoppingcarts != null)
                {
                    //Cycle through each ShoppingCart record in the ShoppingCarts list
                    foreach (var shopping in shoppingcarts)
                    {
                        db.ShoppingCarts.Remove(shopping);
                    }
                }

                //Saves both Orders and ShoppingCart changes back to db (notice how we only need one for both statements)
                db.SaveChanges();
            }

            //Jason wouldn't do this, he'd probably return to some kind of receipt page. or return View("OrderConfirm");
            return View();
        }

        //GET: OrderConfirm
        public ActionResult OrderConfirm()
        {
            return View();
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        //In order to place an order we really need Address, City etc. information. We are going to use the GET for that
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Method declaration returning a type ActionResult, 
        public ActionResult Create([Bind(Include = "Address,City,State,ZipCode,Country,Phone")] Order order)
        {
            //Go out to database, users table, and find record that has this id, but return the whole record & set it equal to user
            var user = db.Users.Find(User.Identity.GetUserId());

            //Grab the records where CustomerId is equal to user.Id
            var shoppingcart = db.ShoppingCarts.Where(s => s.CustomerId == user.Id).ToList();

            //variable of type decimal that will cylce through the items in our list
            decimal totalAmt = 0;
            if (shoppingcart.Count != 0)
            {
                if (ModelState.IsValid)
                {
                    foreach (var product in shoppingcart)
                    {
                        OrderDetail orderdetail = new OrderDetail();
                        orderdetail.ItemId = product.ItemId;
                        orderdetail.OrderId = order.Id;
                        orderdetail.Quantity += product.Count;
                        orderdetail.UnitPrice = product.Item.Price;
                        totalAmt += (product.Count * product.Item.Price);

                        db.OrderDetails.Add(orderdetail);
                    }

                    order.Total = totalAmt;
                    order.Completed = true;
                    order.OrderDate = DateTime.Now;
                    order.CustomerId = user.Id;
                    db.Orders.Add(order);
                    db.SaveChanges();

                    //Call the Complete method here to remove all shopping 
                    this.Complete(order.Id);

                    var myOrder = db.Orders.Where(o => o.Id == order.Id).Include("Customer").Single();
                    return View("Receipt", myOrder);
                }
            }
            //ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", order.CustomerId);
            ViewBag.NoItem = "There's no item to order";
            return View(order);
        }

        // GET: Orders/Edit/5
        //Creating an ActionResult called Edit w/ nullable int parameter called id 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,City,State,ZipCode,Country,Phone,OrderDate,CustomerId,Total,Completed")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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

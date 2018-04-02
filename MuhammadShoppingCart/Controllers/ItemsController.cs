using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MuhammadShoppingCart.Models;
using MuhammadShoppingCart.Helper;
using System.IO;

namespace MuhammadShoppingCart.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ImageUploadValidator validator = new ImageUploadValidator();

        // GET: Items
        //Returns ALL watches
        //[Authorize]
        public ActionResult Index()
        {
            return View(db.Items.ToList());
        }

        //Returns Mens watches
        //[Authorize]
        public ActionResult MensIndex()
        {
            //Additional "Where" clause has been added in to limit the set of data coming in
            return View("Index", db.Items.Where(w => w.Gender == "M").ToList());
        }

        //Returns Womens watches
        //[Authorize]
        public ActionResult WomensIndex()
        {
            return View("Index", db.Items.Where(w => w.Gender == "W").ToList());
        }

        //Returns Womens watches
        //[Authorize]
        public ActionResult SportsIndex()
        {
            return View("Index", db.Items.Where(w => w.Gender == "S").ToList());
        }

        public ActionResult CheapestIndex()
        {
            return View("Index", db.Items.Where(p => p.Price < 250).ToList());
        }

        public ActionResult CheaperIndex()
        {
            return View("Index", db.Items.Where(p => p.Price >= 250 && p.Price < 1000).ToList());
        }

        public ActionResult CheapIndex()
        {
            return View("Index", db.Items.Where(p => p.Price >= 1000 && p.Price < 5000).ToList());
        }

        public ActionResult ExpensiveIndex()
        {
            return View("Index", db.Items.Where(p => p.Price >= 5000).ToList());
        }


        // GET: Items/Details/5
        //[Authorize (Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //We're setting this item object that is an instance of item and giving it a value of a database
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            //The following line returns the "Create" View
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        //These square bracket things are called a data annotation, the [HttpPost] allows this "Create" to be of the "Post" variety. Without it, it's assumed that it's an HttpGet
        [HttpPost]
        [ValidateAntiForgeryToken] //Another security measure 
        public ActionResult Create([Bind(Include = "Name,Price,MediaUrl,Description,Gender")] Item item, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                ImageUploadValidator validator = new ImageUploadValidator();
                    if (ImageUploadValidator.IsWebFriendlyImage(Image))
                    {
                        var fileName = Path.GetFileName(Image.FileName);
                        Image.SaveAs(Path.Combine(Server.MapPath("~/images/uploads/"), fileName));
                        item.MediaUrl = "~/images/uploads/" + fileName;
                    }
                    item.Created = DateTime.Now;
                    //Attaches this particular item to the db set (db is an instance of ApplicationDbContext), the next one stores it back to SQL.
                    db.Items.Add(item);
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        //[Authorize (Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Going out to the items table in the database finding id and assigning it to item. 
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,MediaUrl,Description,Created,Updated")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        //[Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Here we're going out and doing some garbage collection and reclaim lost memory. This is done automatically whenever it can
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

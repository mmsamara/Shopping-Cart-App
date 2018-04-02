using Microsoft.AspNet.Identity;
using MuhammadShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuhammadShoppingCart.Helper
{
    public class CartHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static decimal CalcCartTotal()
        {
            var subTotal = 0m;

            //TODO: How do I reference the currently logged in user outside of a Controller?
            //User.Identity.GetUserId(), but since were not in the controller...
            var userId = HttpContext.Current.User.Identity.GetUserId();

            //Go get all the ShoppingCart records that point back to me...
            var myCarts = db.ShoppingCarts.AsNoTracking().Where(s => s.CustomerId == userId).ToList();
            foreach(var cart in myCarts)
            {
                //Refer to cart props
                subTotal += cart.Item.Price * cart.Count;
            }

            return subTotal;
        }

        public static int CartCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var itemCount = 0;
            var myCarts = db.ShoppingCarts.AsNoTracking().Where(s => s.CustomerId == userId).ToList();
            foreach(var cart in myCarts)
            {
                itemCount += cart.Count;
            }
            return itemCount;
        }


    }
}
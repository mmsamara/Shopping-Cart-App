using Microsoft.AspNet.Identity;
using MuhammadShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuhammadShoppingCart.Helper
{
    public class OrderHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static int OrderHistoryCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var count = 0;
            if (userId != null)
            {
                return 0;
            }
            return count;
        }
    }
}
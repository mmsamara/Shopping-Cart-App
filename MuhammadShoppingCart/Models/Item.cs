using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuhammadShoppingCart.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string MediaUrl { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        //Navigation Properties
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public Item()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.ShoppingCarts = new HashSet<ShoppingCart>();
        }

    }
}
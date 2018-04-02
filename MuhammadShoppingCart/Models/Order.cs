using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuhammadShoppingCart.Models
{
    public class Order
    {
        //Add the necessary properties
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public decimal Total { get; set; }
        public bool Completed { get; set; }

        //Navigation Properties

        //This virtual property is here because Order is the Parent of OrderDetail
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        //This virtual property is here because Order is the Child of AspNetUsers
        public virtual ApplicationUser Customer { get; set; }

        public Order()
        {
            //We need one of these lines for each ICollection type property
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    }
}
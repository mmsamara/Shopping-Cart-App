namespace MuhammadShoppingCart.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MuhammadShoppingCart.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MuhammadShoppingCart.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MuhammadShoppingCart.Models.ApplicationDbContext context)
        {
            #region Account Code
            //Go to view > task list to see the TODO's
            //TODO: Write code to create a new Role of 'Admin'
            var roleManager = new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

            //Check for the existense of a Role with the name Admin, & create one if it's not already present
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin"});
            }

            //TODO: Write code to create a new User (i.e. William, Meghan, Muhammad etc...)
            var userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            if(!context.Users.Any(user => user.Email == "momosamara@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "momosamara@gmail.com",
                    Email = "momosamara@gmail.com",
                    FirstName = "Muhammad",
                    LastName = "Samara",
                    DisplayName = "Muhammad"
                }, "Samara4U123");
            }

            //TODO: Write code that assigns the Admin role to the newly added user
            var userId = userManager.FindByEmail("momosamara@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");
            #endregion

            #region Seed Item Table
            context.Items.AddOrUpdate(
                p => p.Name,
                    new Item { Created = DateTime.Now, Name = "Men's Seeded Watch #1", Description = "This is the Description for the watch...", Price = 125.00m, Gender = "M", MediaUrl = "~/images/m1.jpg" },
                    new Item { Created = DateTime.Now, Name = "Men's Seeded Watch #2", Description = "This is the Description for the watch...", Price = 500.00m, Gender = "M", MediaUrl = "~/images/m2.jpg" },
                    new Item { Created = DateTime.Now, Name = "Men's Seeded Watch #3", Description = "This is the Description for the watch...", Price = 2000.00m, Gender = "M", MediaUrl = "~/images/m3.jpg" },
                    new Item { Created = DateTime.Now, Name = "Men's Seeded Watch #4", Description = "This is the Description for the watch...", Price = 9000.00m, Gender = "M", MediaUrl = "~/images/m4.jpg" },
                    new Item { Created = DateTime.Now, Name = "Men's Seeded Watch #5", Description = "This is the Description for the watch...", Price = 35000.00m, Gender = "M", MediaUrl = "~/images/m5.jpg" },
                    new Item { Created = DateTime.Now, Name = "Women's Seeded Watch #1", Description = "This is the Description for the watch...", Price = 100.00m, Gender = "W", MediaUrl = "~/images/g1.jpg" },
                    new Item { Created = DateTime.Now, Name = "Women's Seeded Watch #2", Description = "This is the Description for the watch...", Price = 125.00m, Gender = "W", MediaUrl = "~/images/g2.jpg" },
                    new Item { Created = DateTime.Now, Name = "Women's Seeded Watch #3", Description = "This is the Description for the watch...", Price = 150.00m, Gender = "W", MediaUrl = "~/images/g6.png" },
                    new Item { Created = DateTime.Now, Name = "Women's Seeded Watch #4", Description = "This is the Description for the watch...", Price = 160.00m, Gender = "W", MediaUrl = "~/images/g4.jpg" },
                    new Item { Created = DateTime.Now, Name = "Women's Seeded Watch #5", Description = "This is the Description for the watch...", Price = 200000.00m, Gender = "W", MediaUrl = "~/images/g5.jpeg" },
                    new Item { Created = DateTime.Now, Name = "Sports Seeded Watch #1", Description = "This is the Description for the watch...", Price = 50.00m, Gender = "S", MediaUrl = "~/images/SportsWatch1Main.jpg" },
                    new Item { Created = DateTime.Now, Name = "Sports Seeded Watch #2", Description = "This is the Description for the watch...", Price = 160.00m, Gender = "S", MediaUrl = "~/images/pumawatch1.jpg" },
                    new Item { Created = DateTime.Now, Name = "Sports Seeded Watch #3", Description = "This is the Description for the watch...", Price = 200000.00m, Gender = "S", MediaUrl = "~/images/fitbit.png" });
            #endregion
        }
    }
}

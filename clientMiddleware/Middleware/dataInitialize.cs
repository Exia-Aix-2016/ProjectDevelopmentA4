using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Middleware
{
    class dataInitialize : System.Data.Entity.DropCreateDatabaseIfModelChanges<userContext>
    {
        protected override void Seed(userContext context)
        {
            var users = new List<User>
            {
                new User{Login = "Wharfan", Password = "root"},
                new User{Login = "Akitoshi", Password = "root"},
                new User{Login = "Noogat", Password = "root"},
                new User{Login = "Hougo13", Password = "root"},
                new User{Login = "BlackWolf", Password = "root"},
                new User{Login = "GetRice", Password = "root"},
                new User{Login = "Luzarox", Password = "root"}
            };
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}

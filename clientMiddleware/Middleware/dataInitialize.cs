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
                new User{Login = "quentin@viacesi.fr", Password = "root"},
                new User{Login = "mandel@viacesi.fr", Password = "root"},
                new User{Login = "baptiste@viacesi.fr", Password = "root"},
                new User{Login = "hugo@viacesi.fr", Password = "root"},
                new User{Login = "luc@viacesi.fr", Password = "root"},
                new User{Login = "denis@viacesi.fr", Password = "root"},
                new User{Login = "ludovic@viacesi.fr", Password = "root"}
            };
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using QRCodeManagement.Models;

namespace QRCodeManagement.Helpers
{
    public class AccountHelper
    {

        public static List<ApplicationUser> GetActiveUsers()
        {
            var context = new AdminDbContext();
            return context.Users.Where(x => x.EmailConfirmed == true 
                                            && !x.Roles.Select(r => r.RoleId).Contains("admin")).ToList();
        }

    }
}
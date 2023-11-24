using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Actual.Models
{
    public static class UserID
    {
        public static int GetIdUser(HttpContext httpContext)
        {
            var claim = httpContext.User.FindFirstValue("UserID");
            if (claim != null && int.TryParse(claim, out int id))
            {
                return id;
            }
            return 0;
        }
    }
}

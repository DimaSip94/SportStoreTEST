using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    public class BaseController : Controller
    {
        private IHttpContextAccessor httpContextAccessor;

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            Program.DisableProfilingResults = httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && httpContextAccessor.HttpContext.User.IsInRole("admin") ? false : true;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class ClaimsController : BaseController
    {
        public ClaimsController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
        [Authorize]
        public ViewResult Index() => View(User?.Claims);
    }
}

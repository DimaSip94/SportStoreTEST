using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class React_ProductController : BaseController
    {
        private IQueueRequests queueRequests;
        public React_ProductController(IQueueRequests queueRequests, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.queueRequests = queueRequests;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Forms()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TestPost(string name, int age)
        {
            queueRequests.SaveFile(name);
            return Json(new { message=$"Hello React {name} {age}"});
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class ChatController:BaseController
    {
        public ChatController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public IActionResult Index() => View();
    }
}

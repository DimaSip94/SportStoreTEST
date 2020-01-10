using Microsoft.AspNetCore.Mvc;
using SportsStore.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent:ViewComponent
    {
        private IEFProductManager eFProductManager;

        public NavigationMenuViewComponent(IEFProductManager eFProductManager)
        {
            this.eFProductManager = eFProductManager;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(eFProductManager.GetCategories());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Managers;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : BaseController
    {
        private IOrderManager orderManager;
        private Cart cart;

        public OrderController(IOrderManager orderManager, Cart cart, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.orderManager = orderManager;
            this.cart = cart;
        }

        public IActionResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                string msg;
                orderManager.SaveOrder(order, out msg);
                if (string.IsNullOrEmpty(msg))
                {
                    return RedirectToAction(nameof(Completed));
                }
                else
                {
                    ModelState.AddModelError("", msg);
                }
            }
            return View(order);
        }

        public  ViewResult Completed()
        {
            cart.Clear();
            return View();
        }

        [Authorize]
        public ViewResult List()
        {
            int total;
            return View(orderManager.GetOrders(out total, 1, 10000000, false));
        }

        [Authorize]
        [HttpPost]
        public IActionResult MarkShipped(int orderID)
        {
            orderManager.MarkShipped(orderID, true);
            return RedirectToAction(nameof(List));
        }
    }
}
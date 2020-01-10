using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Managers;
using SportsStore.Models;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IEFProductManager repository;
        private Cart cart;

        public CartController(IEFProductManager _repository, Cart _cart)
        {
            repository = _repository;
            cart = _cart;
        }

        public RedirectToActionResult AddToCart(int productID, string returnUrl)
        {
            Product product = repository.GetProduct(productID);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(string returnUrl)
        {
            CartindexViewModel cartindexViewModel = new CartindexViewModel {
             cart = cart,
             ReturnUrl = returnUrl
            };
            return View(cartindexViewModel);
        }

        public RedirectToActionResult RemoveItemFromCart(int productID, string returnUrl)
        {
            Product product = repository.GetProduct(productID);
            if (product != null)
            {
                cart.RemoveItem(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

    }
}
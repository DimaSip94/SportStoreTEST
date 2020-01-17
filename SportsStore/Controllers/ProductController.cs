using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Managers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using SportsStore.Repositories;

namespace SportsStore.Controllers
{
    public class ProductController : BaseController
    {
        private IProductRepository productRepository;
        private IEFProductManager eFProductManager;
        private int pageSize = 5;

        public ProductController(IProductRepository productRepository, IEFProductManager eFProductManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.productRepository = productRepository;
            this.eFProductManager = eFProductManager;
        }         

        public ViewResult List(string category, int productPage = 1)
        {
            int total;
            ProductsListViewModel productsListViewModel = new ProductsListViewModel {
                Products = eFProductManager.GetProducts(out total, category, productPage, pageSize),
                PagingInfo = new PagingInfo { CurrentPage= productPage, ItemsPerPage = pageSize, TotalItems = total },
                CurrentCategory = category
            };
            return View(productsListViewModel);
        }
    }
}
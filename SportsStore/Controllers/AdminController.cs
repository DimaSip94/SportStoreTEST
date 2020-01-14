using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SportsStore.Managers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IEFProductManager repo;
        private UserManager<IdentityUser> userManager;
        private IConfiguration configuration;
        private IFileManager fileManager;
        private IHostingEnvironment env;
        private IMapper mapper;

        private string uploadsFile = "";
        private string tempFile = "";

        public AdminController(IEFProductManager repo, UserManager<IdentityUser> userManager, IConfiguration configuration, IFileManager fileManager, IHostingEnvironment env, IMapper mapper)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.configuration = configuration;
            this.fileManager = fileManager;
            this.env = env;
            this.mapper = mapper;

            uploadsFile = configuration.GetSection("FileSystem").GetSection("Uploads").Value;
            tempFile = configuration.GetSection("FileSystem").GetSection("Temp").Value;
        }
        public IActionResult Index()
        {
            int total;
            List<Product> products = repo.GetProducts(out total);
            List<ProductViewModel> model = new List<ProductViewModel>();
            foreach (var p in products)
            {
                ProductViewModel m = mapper.Map<Product, ProductViewModel>(p);
                model.Add(m);
            }
            return View(model);
        }

        public IActionResult Edit(int productid)
        {
            Product product = repo.GetProduct(productid);
            ProductViewModel model = mapper.Map<Product, ProductViewModel>(product);
            return View();
        }

        public IActionResult Create()
        {
            return View("Edit", new ProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.LogoFile != null)
                {
                    string absPath = uploadsFile + "/" + Guid.NewGuid().ToString() + Path.GetExtension(model.LogoFile.FileName);
                    string filePath = env.WebRootPath + absPath;
                    string savePath = await fileManager.SaveFileAsync(model.LogoFile, filePath);
                    model.LogoPath = string.IsNullOrEmpty(savePath) ? "" : absPath;
                }
                Product product = mapper.Map<ProductViewModel, Product>(model);
                var res = repo.SaveProduct(product);
                if (res.IsSuccess)
                {
                    TempData["message"] = $"{model.Name} has been saved";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", res.ErrorMessage);
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int ProductID)
        {

            var res = repo.DeleteProduct(ProductID);
            if (res.IsSuccess)
            {
                TempData["message"] = "Product has been deleted";
            }
            else
            {
                TempData["error"] = res.ErrorMessage;
            }
            return RedirectToAction("Index");
        }

        public ViewResult Users()
        {
            return View(userManager.Users.ToList());
        }
    }
}
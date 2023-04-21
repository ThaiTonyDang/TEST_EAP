using DangTranThai_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DangTranThai_WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Home Page";
            var list = await GetProducts();
            if (list != null)       // Nếu list user khác null thì trả về View có chứa list 
                return View(list);
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            string baseUrl = GetBaseUrl();
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.PostAsJsonAsync(baseUrl + "api/product", product).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(product);
        }

        public ActionResult Edit(int id)
        {
            var dbContext = new ProductDbContext();
            var product = dbContext.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Product product)
        {
            string baseUrl = GetBaseUrl();
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.PutAsJsonAsync(baseUrl + "api/product", product).Result;
                //response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(product);
        }

        public async Task<ActionResult> Delete(int id)
        {
            string baseUrl = GetBaseUrl();
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.DeleteAsync(baseUrl + "api/product/" + id).Result;
                //response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        private async Task<List<Product>> GetProducts()
        {
            string baseUrl = GetBaseUrl();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(baseUrl + "api/product");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var list = new List<Product>();
                    list = response.Content.ReadAsAsync<List<Product>>().Result; return list;
                }
                return null;
            }
        }

        private string GetBaseUrl()
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            return baseUrl;
        }
    }  
}

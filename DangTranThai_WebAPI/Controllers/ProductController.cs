using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DangTranThai_WebAPI.Controllers
{
    public class ProductController : ApiController
    {
        [HttpGet]
        public IEnumerable GetProducts()
        {
            using (ProductDbContext dataContext = new ProductDbContext())
            {
                return dataContext.Products.ToList();
            }
        }

        [HttpPost]
        public IHttpActionResult CreateNew(Product product)
        {
            try
            {
                using (ProductDbContext dataContext = new ProductDbContext())
                {
                    dataContext.Products.Add(product);
                    dataContext.SaveChanges();
                    var reponse = Request.CreateResponse(HttpStatusCode.Created, product);
                    reponse.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = product.ProductId }));
                    return Ok();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            using (ProductDbContext dbContext = new ProductDbContext())
            {
                var productEntity = dbContext.Products.Where(p=> p.ProductId == product.ProductId).FirstOrDefault();
                if (productEntity != null)
                {
                    productEntity.ProductId = id;
                    productEntity.ProductName = product.ProductName;
                    productEntity.UnitPrice = product.UnitPrice;

                    dbContext.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
                return base.Ok((Product)productEntity);
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteEmployee(int id)
        {
            using (ProductDbContext dbContext = new ProductDbContext())
            {
                var productEntity = GetProductById(dbContext, id);
                if (productEntity != null)
                {
                    dbContext.Products.Remove(productEntity);
                    dbContext.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
                return Ok(dbContext);
            }

        }

        private Product GetProductById(ProductDbContext dbContext, int id)
        {
            var productEntity = dbContext.Products.Where(p => p.ProductId == id).FirstOrDefault();

            return productEntity;
        }
    }
}

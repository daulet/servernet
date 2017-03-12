using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        readonly Product[] _products = {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };

        [Route("")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        [Route("{id}")]
        public IHttpActionResult GetProduct(int id)
        {
            var product = _products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
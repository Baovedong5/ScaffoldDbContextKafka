using Microsoft.EntityFrameworkCore;
using ProductServices.Core.InMemory;
using ProductServices.Models;

namespace ProductServices.Core
{
    public class TableProductMemorySeedAsync
    {
        public async Task SeedAsync(ProductMemory memory, ModelContext context)
        {
            var products = await context.TableProducts.ToListAsync();

            if(products.Count > 0)
            {
                foreach(var product in products)
                {
                    memory.ProductMem.Add(product.Id.ToString(), product);
                }
            }
        }
    }
}

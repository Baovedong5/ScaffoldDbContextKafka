using ProductServices.Models;

namespace ProductServices.Core.InMemory
{
    public class ProductMemory
    {
        public Dictionary<string, TableProduct> ProductMem {  get; set; }

        public ProductMemory()
        {
            ProductMem = new Dictionary<string, TableProduct>();
        }
    }
}

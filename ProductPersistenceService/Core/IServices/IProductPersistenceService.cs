
using ProductPersistenceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPersistenceServices.Core.IServices
{
    internal interface IProductPersistenceService
    {
        Task<TableProduct> InsertAsync(TableProduct product);

        Task<TableProduct> UpdatePriceAsync(int productId, decimal price);

        Task<TableProduct> UpdateQuantityAsync(int productId, int quantity);
    }
}

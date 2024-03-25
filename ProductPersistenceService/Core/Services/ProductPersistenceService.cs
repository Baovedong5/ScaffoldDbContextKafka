using Microsoft.EntityFrameworkCore;

using ProductPersistenceServices.Core.IServices;
using ProductPersistenceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPersistenceServices.Core.Services
{
    internal class ProductPersistenceService : IProductPersistenceService
    {
        private readonly ModelContext _context;

        public ProductPersistenceService(ModelContext context)
        {
            _context = context;
        }

        public async Task<TableProduct> InsertAsync(TableProduct product)
        {
            try
            {
                await _context.TableProducts.AddAsync(product);

                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TableProduct> UpdatePriceAsync(int productId, decimal price)
        {
            try
            {
                var product = await _context.TableProducts.FirstOrDefaultAsync(x => x.Id == productId);

                product.Price = price;

                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TableProduct> UpdateQuantityAsync(int productId, int quantity)
        {
            try
            {
                var product = await _context.TableProducts.FirstOrDefaultAsync(x => x.Id == productId);

                product.Quantity = quantity;

                await _context.SaveChangesAsync();

                return product;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

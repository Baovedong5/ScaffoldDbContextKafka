using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;
using ProductPersistenceServices.Core.IServices;

using ProductPersistenceServices.DTOs;
using ProductPersistenceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductPersistenceServices.BackgroundTasks
{
    internal class ProductPersistenceConsuming : IConsumingTask<string, string>
    {
        private readonly IProductPersistenceService _productPersistenceService;

        public ProductPersistenceConsuming(IProductPersistenceService productPersistenceService)
        {
            _productPersistenceService = productPersistenceService;
        }

        public void Execute(ConsumeResult<string, string> result)
        {
            var productEvent = "";

            foreach (var header in result.Message.Headers)
            {
                productEvent = Encoding.UTF8.GetString(header.GetValueBytes());
            }

            if (productEvent == "InsertProduct")
            {
                var product = JsonSerializer.Deserialize<TableProduct>(result.Message.Value);

                _productPersistenceService.InsertAsync(product);
            }
            else if (productEvent == "UpdateQuantity")
            {
                var product = JsonSerializer.Deserialize<UpdateQuantityDto>(result.Message.Value);
                _productPersistenceService.UpdateQuantityAsync(product.Id, product.Quantity);
            }
            else if (productEvent == "UpdatePrice")
            {
                var product = JsonSerializer.Deserialize<UpdatePriceDto>(result.Message.Value);
                _productPersistenceService.UpdatePriceAsync(product.Id, product.Price);
            }
        }
    }
}

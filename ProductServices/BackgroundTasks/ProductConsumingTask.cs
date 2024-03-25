using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;
using ProductServices.Core.IServices;
using ProductServices.DTOs;
using ProductServices.Models;
using System.Text;
using System.Text.Json;

namespace ProductServices.BackgroundTasks
{
    public class ProductConsumingTask : IConsumingTask<string, string>
    {

        private readonly IProductService _productService;

        public ProductConsumingTask(IProductService productService)
        {
            _productService = productService;
        }

        public void Execute(ConsumeResult<string, string> result)
        {
            var productEvent = "";

            foreach (var header in result.Message.Headers) 
            {
                productEvent = Encoding.UTF8.GetString(header.GetValueBytes());
            }

            if(productEvent == "InsertProduct")
            {
                var product = JsonSerializer.Deserialize<TableProduct>(result.Message.Value);
                _productService.Insert(product);
            }
            else if(productEvent == "UpdatePrice")
            {
                var product = JsonSerializer.Deserialize<UpdatePriceDto>(result.Message.Value);
                _productService.UpdatePrice(product.Id, product.Price);
            }
            else if (productEvent == "UpdateQuantity")
            {
                var product = JsonSerializer.Deserialize<UpdateQuantityDto>(result.Message.Value);
                _productService.UpdateQuantity(product.Id, product.Quantity, product.Increase);
            }
        }
    }
}

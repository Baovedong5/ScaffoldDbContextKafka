using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;
using ProductServices.Core.InMemory;
using ProductServices.Core.IServices;
using ProductServices.Models;
using System.Text;
using System.Text.Json;

namespace ProductServices.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductMemory _inMem;

        private readonly ILogger<ProductService> _logger;

        private readonly IKafkaProducerManager _producerManager;

        public ProductService(ProductMemory inMem, ILogger<ProductService> logger, IKafkaProducerManager producerManager)
        {
            _inMem = inMem;
            _logger = logger;
            _producerManager = producerManager;
        }

        public List<TableProduct> All()
        {
            var products = _inMem.ProductMem.Values.ToList();

            return products;
        }

        public TableProduct Insert(TableProduct product)
        {
            try
            {
                _inMem.ProductMem.Add(product.Id.ToString(), product);

                var kafkaProducer = _producerManager.GetProducer<string, string>("1");

                var message = new Message<string, string>
                {
                    Key = "emptyornull",
                    Value = JsonSerializer.Serialize(product),
                    Headers = new Headers
                    {
                        {
                            "eventname", Encoding.UTF8.GetBytes("InsertProduct")
                        }
                    }
                };

                kafkaProducer.Produce(message);

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public TableProduct? UpdatePrice(int productId, decimal price)
        {
            try
            {
                var product = _inMem.ProductMem.FirstOrDefault(x => x.Value.Id == productId).Value;

                if(product != null)
                {
                    if(price < 0)
                    {
                        _logger.LogError("price must be greater than 0");
                    }
                    else
                    {
                        product.Price = price;

                        var kafkaProducer = _producerManager.GetProducer<string,string>("1");

                        var message = new Message<string, string>
                        {
                            Key = productId.ToString(),
                            Value = JsonSerializer.Serialize(product),
                            Headers = new Headers
                            {
                                {
                                    "eventname", Encoding.UTF8.GetBytes("UpdatePrice")
                                }
                            }
                        };

                        kafkaProducer.Produce(message);
                    }

                    return product;
                }
                else
                {
                    _logger.LogError("Product is not exist");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public TableProduct? UpdateQuantity(int productId, int quantity, bool increase)
        {
            try
            {
                var product = _inMem.ProductMem.FirstOrDefault(x => x.Value.Id == productId).Value;
                if(product != null)
                {
                    if(increase)
                    {
                        product.Quantity += quantity;
                    }
                    else
                    {
                        if(product.Quantity < quantity)
                        {
                            product.Quantity = product.Quantity;
                            _logger.LogError("nagative quantity");
                        }
                        else
                        {
                            product.Quantity -= quantity;
                        }
                    }
                    var kafkaProducer = _producerManager.GetProducer<string, string>("1");

                    var message = new Message<string, string>
                    {
                        Key = productId.ToString(),
                        Value = JsonSerializer.Serialize(product),
                        Headers = new Headers
                        {
                              {
                                    "eventname", Encoding.UTF8.GetBytes("UpdateQuantity")
                              }
                        }
                    };

                    kafkaProducer.Produce(message);
                    return product;
                }
                else
                {
                    _logger.LogError("Product is not exist");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}

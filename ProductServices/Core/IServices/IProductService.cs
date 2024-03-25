using ProductServices.Models;

namespace ProductServices.Core.IServices
{
    public interface IProductService
    {
        List<TableProduct> All();

        TableProduct Insert(TableProduct product);

        TableProduct? UpdatePrice(int productId, decimal price);

        TableProduct? UpdateQuantity(int productId, int quantity, bool increase);
    }
}

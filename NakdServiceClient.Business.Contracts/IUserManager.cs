using NakdServiceClient.Domain;
using System.Threading.Tasks;

namespace NakdServiceClient.Business.Contracts
{
    public interface ICustomerManager
    {
        Task<string> CreateCustomerAsync(Customer customer);

        Task<bool> AddProductToWishListAsync(string customerId, Product product);

        Task<bool> DeleteProductFromWishListAsync(string customerId, string productId);
    }
}

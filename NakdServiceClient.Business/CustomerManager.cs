using NakdServiceClient.Business.Contracts;
using NakdServiceClient.Business.Contracts.Services;
using NakdServiceClient.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NakdServiceClient.Business
{
    public class CustomerManager : ICustomerManager
    {
        private IHttpRestHelper _httpRestHelper;
        private const string CreateCustomerResource = "/v1/customers";
        private const string AddProductResource = "/v1/customers/{id}/wishListProducts";
        private const string DeleteProductResource = "/v1/customers/{id}/wishListProducts/{productId}";

        public CustomerManager(IHttpRestHelper httpRestHelper)
        {
            _httpRestHelper = httpRestHelper;
        }

        public async Task<string> CreateCustomerAsync(Customer customer)
        {
            var errorMessages = ValidateCustomer(customer);
            if (errorMessages.Any())
            {
                throw new ArgumentException($"Validation Errors: {string.Join("#", errorMessages)}");
            }
            else
            {
                var customerResult = await _httpRestHelper.DoPostAsync<Customer, CustomerResult>(CreateCustomerResource, customer);
                return customerResult?.id;
            }
        }

        public async Task<bool> AddProductToWishListAsync(string customerId, Product product)
        {
            if (product == null || !Guid.TryParse(customerId, out var guid))
            {
                throw new System.ArgumentException("The customer id should be in GUID format and the product shouldn't be empty");
            }
            else
            {
                var urlSegments = new List<Tuple<string, string>>()
                {
                    Tuple.Create<string,string>("id", customerId)
                };

                return await _httpRestHelper.DoPostAsync(AddProductResource, product, urlSegments);
            }
        }

        public async Task<bool> DeleteProductFromWishListAsync(string customerId, string productId)
        {
            if (!Guid.TryParse(productId, out var guid) || !Guid.TryParse(customerId, out var guid2))
            {
                throw new ArgumentException("The customer id and the productId should be in GUID format");
            }
            else
            {
                var urlSegments = new List<Tuple<string, string>>()
                {
                    Tuple.Create<string,string>("id", customerId),
                    Tuple.Create<string,string>("productId", productId),
                };

                return await _httpRestHelper.DoDeleteAsync<Product>(DeleteProductResource, urlSegments);
            }
        }

        private List<string> ValidateCustomer(Customer customer)
        {
            List<string> errors = new();
            if (customer == null)
            {
                errors.Add("Error trying Customer with empty data");
            }
            if (!Guid.TryParse(customer?.TenantId, out var guid))
            {
                errors.Add("TenantId Should be in GUID format.");
            }
            return errors;
        }
    }
}

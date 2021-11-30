using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NakdServiceClient.Business.Contracts.Services;
using NakdServiceClient.Domain;
using Shouldly;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NakdServiceClient.Business.Tests.Unit
{
    [TestClass]
    public class CustomerManagerTests
    {
        private Mock<IHttpRestHelper> _mockHttpRestHelper;
        private CustomerManager _customerManager;

        private const string CustomerId =   "3F2504E0-4F89-11D3-9A0C-0305E82C3301";
        private const string ProductId =    "3F2504E0-4F89-11D3-9A0C-0305E82C3971";

        public CustomerManagerTests()
        {
            _mockHttpRestHelper = new Mock<IHttpRestHelper>();
            _customerManager = new CustomerManager(_mockHttpRestHelper.Object);
        }

        [TestMethod]
        public async Task CreateCustomerWithInvalidValuesShouldThrow()
        {
            var customer = new Customer();

            _mockHttpRestHelper.Setup(helper => helper.DoPostAsync<Customer, CustomerResult>(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<IList<Tuple<string, string>>>()))
               .ReturnsAsync(new CustomerResult() { id = CustomerId });

            await Should.ThrowAsync<ArgumentException>(async () => await _customerManager.CreateCustomerAsync(customer));
        }

        [TestMethod]
        public async Task CreateCustomerWithNullDataShouldThrowException()
        {
            Customer customer = null;
            _mockHttpRestHelper.Setup(helper => helper.DoPostAsync<Customer, string>(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<IList<Tuple<string, string>>>()));

            await Should.ThrowAsync<ArgumentException>(async () => await _customerManager.CreateCustomerAsync(customer));
        }

        [TestMethod]
        public async Task CreateCustomerWithValidValuesShouldCreate()
        {
            Customer customer = new Customer() { TenantId = Guid.NewGuid().ToString(), Name = "test", Description = "desc", Enabled = true };
            _mockHttpRestHelper.Setup(helper => helper.DoPostAsync<Customer, CustomerResult>(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<IList<Tuple<string, string>>>()))
                .ReturnsAsync(new CustomerResult() { id = CustomerId });

            var result = await _customerManager.CreateCustomerAsync(customer);
            result.ShouldBe(CustomerId);
        }

        [TestMethod]
        public async Task AddProductToWishListWithEmptyCustomerIdShouldThrowException()
        {
            Product product = new();
            await Should.ThrowAsync<ArgumentException>(async () => await _customerManager.AddProductToWishListAsync(string.Empty, product));
        }

        [TestMethod]
        public async Task AddProductToWishListWithNullProductShouldThrowException()
        {
            Product product = null;

            await Should.ThrowAsync<ArgumentException>(async () => await _customerManager.AddProductToWishListAsync(CustomerId, product));
        }

        [TestMethod]
        public async Task AddProductToWishListWithValidValuesShouldAddProduct()
        {
            Product product = new() { Id = ProductId, Name = "product 1", Description = "test product" };

            _mockHttpRestHelper.Setup(helper => helper.DoPostAsync<Product>(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<IList<Tuple<string, string>>>())).ReturnsAsync(true);

            var result = await _customerManager.AddProductToWishListAsync(CustomerId, product);

            result.ShouldBeTrue();
        }

        [TestMethod]
        public async Task DeleteProductFromWishListWithEmptyCustomerIdShouldThrowException()
        {
            await Should.ThrowAsync<ArgumentException>(async () => await _customerManager.DeleteProductFromWishListAsync(string.Empty, ProductId));
        }

        [TestMethod]
        public async Task DeleteProductToWishListWithNullProductShouldThrowException()
        {
            await Should.ThrowAsync<ArgumentException>(async () => await _customerManager.DeleteProductFromWishListAsync(CustomerId, string.Empty));
        }

        [TestMethod]
        public async Task DeleteProductToWishListWithValidValuesShouldAddProduct()
        {
            _mockHttpRestHelper.Setup(helper => helper.DoDeleteAsync<Product>(It.IsAny<string>(), It.IsAny<IList<Tuple<string, string>>>())).ReturnsAsync(true);

            await Should.NotThrowAsync(async () => await _customerManager.DeleteProductFromWishListAsync(CustomerId, ProductId));
        }
    }
}

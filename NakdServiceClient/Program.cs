using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NakdServiceClient.Business;
using NakdServiceClient.Business.Contracts;
using NakdServiceClient.Business.Contracts.Services;
using NakdServiceClient.Domain;
using NakdServiceClient.Services;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NakdServiceClient
{
    class Program
    {
        private static ICustomerManager _customerManager;

        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args)
            .ConfigureServices((_, services) => InitializeServices(services))
            .Build();

            var Configuration = host.Services.GetService<IConfiguration>();
            var baseUrl = Configuration.GetValue<string>("BaseUrl");
            var restClient = host.Services.GetService<IRestClient>();
            restClient.BaseUrl = new Uri(baseUrl);

            _customerManager = host.Services.GetService<ICustomerManager>();

            MainMenu();

            await host.RunAsync();
        }

        private static void MainMenu()
        {
            char keyChar = char.MinValue;

            while (keyChar != '0')
            {
                try
                {
                    Console.WriteLine("\r\n-----------------------------------------------");
                    Console.WriteLine("0: Exit");
                    Console.WriteLine("1: Create Customer ");
                    Console.WriteLine("2: Add product to customer wishlist");
                    Console.WriteLine("3: Remove product from customer wishlist");

                    Console.Write("\r\nChoose option:");
                    var key = Console.ReadKey();
                    keyChar = key.KeyChar;
                    var options = new char[] { '0', '1', '2', '3' };
                    if (options.Any(validOption => keyChar == validOption))
                    {
                        Console.WriteLine("\r\nPerforming selected option ....\r\n");
                        PerformOption(key.KeyChar);
                    }
                    else
                    {
                        Console.WriteLine("\r\nInvalid opton, please try again.");
                    }
                    Console.WriteLine("\r\n-----------------------------------------------\r\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }
        }

        private static void PerformOption(char key)
        {
            var customerId = string.Empty;
            var productId = string.Empty;

            switch (key)
            {
                case '0':
                    Console.WriteLine("\r\nBye\r\n");
                    break;
                case '1':
                    var customer = new Customer();
                    Console.Write("\tCustomer Name: ");
                    customer.Name = Console.ReadLine();
                    Console.Write("\tCustomer tenantId (GIUD): ");
                    customer.TenantId = Console.ReadLine();
                    Console.Write("\tCustomer description: ");
                    customer.Description = Console.ReadLine();
                    customer.Enabled = true;

                    customerId = _customerManager.CreateCustomerAsync(customer).Result;

                    Console.WriteLine($"\r\nCustomer created: customerId {customerId}");

                    break;
                case '2':
                    var product = new Product();
                    Console.Write("\tCustomer Id (GIUD): ");
                    customerId = Console.ReadLine();
                    Console.Write("\tProduct Name: ");
                    product.Name = Console.ReadLine();
                    Console.Write("\tProduct Id (GIUD): ");
                    product.Id = Console.ReadLine();
                    Console.Write("\tProduct Description: ");
                    product.Description = Console.ReadLine();                   

                    var added = _customerManager.AddProductToWishListAsync(customerId, product).Result;

                    Console.WriteLine($"\r\nProduct added to customerId {customerId}: {added}");
                    break;
                case '3':
                    Console.Write("\tCustomer Id (GIUD): ");
                    customerId = Console.ReadLine();
                    Console.Write("\tProduct Id (GIUD): ");
                    productId = Console.ReadLine();

                    var deleted = _customerManager.DeleteProductFromWishListAsync(customerId, productId).Result;

                    Console.WriteLine($"\r\nProduct deleted {deleted}: customerId {customerId}, productId {productId}");
                    break;
                default:
                    break;
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);

        public static void InitializeServices(IServiceCollection services)
        {
            services.AddTransient<ICustomerManager, CustomerManager>();
            services.AddScoped<IHttpRestHelper, HttpRestHelper>();
            services.AddSingleton<IRestClient, RestClient>();
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NakdServiceClient.Domain;
using NakdServiceClient.Services;
using RestSharp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NakdServiceClient.Business.Tests.Unit.Services
{
    [TestClass]
    public class RestHelperTests
    {
        private Mock<IRestClient> mockRestClient;
        private HttpRestHelper restHelper;

        public RestHelperTests()
        {
            mockRestClient = new Mock<IRestClient>();
            restHelper = new HttpRestHelper(mockRestClient.Object);
        }

        [TestMethod]
        public async Task DoPostWitoutBaseUrlShouldThrowApplicationException()
        {
            var Customer = new Customer() { };
            await Should.ThrowAsync<ApplicationException>(async () => await restHelper.DoPostAsync<Customer, string>("reource", Customer));
        }

        [TestMethod]
        public async Task DoPostWitoutResourceShouldThrowApplicationException()
        {
            var Customer = new Customer() { };
            restHelper.BaseUrl = new Uri("Http://testuri.com");
            await Should.ThrowAsync<ApplicationException>(async () => await restHelper.DoPostAsync<Customer, string>("reource", Customer));
        }

        [TestMethod]
        public async Task DoDeleteWitoutBaseUrlShouldThrowApplicationException()
        {
            var Customer = new Customer() { };
            var urlSegments = new List<Tuple<string, string>>()
            {
                Tuple.Create<string, string>("param1","2"),
                Tuple.Create<string, string>("param2","3"),
            };

            await Should.ThrowAsync<ApplicationException>(async () => await restHelper.DoDeleteAsync<Customer>("reource", urlSegments));
        }

        [TestMethod]
        public async Task DoDeleteWitoutResourceShouldThrowApplicationException()
        {
            var Customer = new Customer() { };
            restHelper.BaseUrl = new Uri("Http://testuri.com");
            var urlSegments = new List<Tuple<string, string>>()
            {
                Tuple.Create<string, string>("param1","2"),
                Tuple.Create<string, string>("param2","3"),
            };

            await Should.ThrowAsync<ApplicationException>(async () => await restHelper.DoDeleteAsync<Customer>(string.Empty, urlSegments));
        }
    }
}

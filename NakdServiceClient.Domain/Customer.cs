using System.Collections.Generic;

namespace NakdServiceClient.Domain
{
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TenantId { get; set; }
        public bool Enabled { get; set; }
        public IList<Product> WishList { get; set; }
    }
}

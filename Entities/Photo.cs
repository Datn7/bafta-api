using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bafta_api.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string PublicId { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}

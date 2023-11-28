using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantWebClient.Web.Models
{
        public class TopDealsProduct
    {
        public required string Id {get; set;}

        public required string Name {get; set;}

        public required double Price { get; set; }

        public required string ImagesUrls {get; set;}

        public required string Url {get; set;}
    }
}
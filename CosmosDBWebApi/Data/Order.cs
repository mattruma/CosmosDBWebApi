using CosmosDBWebApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CosmosDBWebApi.Data
{
    public class Order
    {
        [SwaggerExclude]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "shopperName")]
        public string ShopperName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "shopperEmail")]
        public string ShopperEmail { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<OrderItem> Items { get; set; }

        public Order()
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<OrderItem>();
        }
    }
}

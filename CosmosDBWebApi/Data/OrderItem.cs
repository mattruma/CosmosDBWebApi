using CosmosDBWebApi.Helpers;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace CosmosDBWebApi.Data
{
    public class OrderItem
    {
        [SwaggerExclude]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty(PropertyName = "quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty(PropertyName = "isTaxable")]
        public bool IsTaxable { get; set; }

        public OrderItem()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

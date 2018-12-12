using CosmosDBWebApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        [JsonProperty(PropertyName = "beforeTaxAmount")]
        public decimal BeforeTaxAmount { get; set; }

        [JsonProperty(PropertyName = "taxAmount")]
        public decimal TaxAmount { get; set; }

        [JsonProperty(PropertyName = "taxRate")]
        public decimal TaxRate { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<OrderItem> Items { get; set; }

        public Order()
        {
            this.BeforeTaxAmount = 0;
            this.TaxAmount = 0;
            this.Amount = 0;
            this.TaxRate = 6.25;
            this.Items = new List<OrderItem>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Api.Models.Mongos
{
    public class AdvertiseFee : BaseEntity
    {
        public String CompanyId { get; set; }
        public String ShopId { get; set; }
        public String Agent { get; set; }
        public String AccountId { get; set; }
        public String ActId { get; set; }
        public String ActName { get; set; }
        public DateTime Date { get; set; }
        public String CurrencyCode { get; set; }
        public decimal CurrencyCodeFee { get; set; }
        public decimal AdvertisementFree { get; set; }
        public decimal HandlingFee { get; set; }
        public int Status { get; set; }
        public int ScrapRemark { get; set; }
        public DateTime? ScrapTime { get; set; }
        public DateTime FbUpdateTime { get; set; }
        public int Platform { get; set; }
    }
}

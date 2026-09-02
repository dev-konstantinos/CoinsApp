using System;
using System.Collections.Generic;
using System.Text;

namespace CoinsApp.DAL.PriceHistory.Models
{
    public sealed class PriceHistoryData
    {
        public int PriceHistoryId { get; set; }

        public int CoinId { get; set; }

        public decimal Price { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyCode { get; set; } = string.Empty;

        public string CurrencyName { get; set; } = string.Empty;

        public DateTime PriceDate { get; set; }

        public string? Source { get; set; }

        public string? Notes { get; set; }
    }
}

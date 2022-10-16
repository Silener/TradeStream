using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TradeStream.WebSocketObjects
{
    // Data returned from the kline/candlestick stream
    public class KlineDetailedData
    {
        [JsonPropertyName("t")]
        public long klineStartTime { get; set; }
        [JsonPropertyName("T")]
        public long klineCloseTime { get; set; }
        [JsonPropertyName("s")]
        public string? symbol { get; set; }
        [JsonPropertyName("i")]
        public string? interval { get; set; }
        [JsonPropertyName("f")]
        public int firstTradeID { get; set; }
        [JsonPropertyName("L")]
        public int lastTradeID { get; set; }
        [JsonPropertyName("o")]
        public string? openPrice { get; set; }
        [JsonPropertyName("c")]
        public string? closePrice { get; set; }
        [JsonPropertyName("h")]
        public string? highPrice { get; set; }
        [JsonPropertyName("l")]
        public string? lowPrice { get; set; }
        [JsonPropertyName("v")]
        public string? baseAssetVolume { get; set; }
        [JsonPropertyName("n")]
        public int numberOfTrades { get; set; }
        [JsonPropertyName("x")]
        public bool isKlineClosed { get; set; }
        [JsonPropertyName("q")]
        public string? quoteAssetVOlume { get; set; }
        [JsonPropertyName("V")]
        public string? takerBuyBaseAssetVolume { get; set; }
        [JsonPropertyName("Q")]
        public string? takerBuyQuoteAssetVolume { get; set; }
        [JsonPropertyName("B")]
        public string? ignored { get; set; }
    }
    public class KlineStreamData
    {
        [JsonPropertyName("e")]
        public string? eventType { get; set; }
        [JsonPropertyName("E")]
        public long eventEpochTIme { get; set; }
        [JsonPropertyName("s")]
        public string? symbol { get; set; }
        [JsonPropertyName("k")]
        public KlineDetailedData? klineDetailedData { get; set; }
    }
}
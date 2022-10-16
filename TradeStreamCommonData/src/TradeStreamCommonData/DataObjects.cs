using System.Text.Json.Serialization;

namespace TradeStreamCommonData.DataObjects
{
    public record DataStreams(int ID, DateTime recordDate, short symbolType, double price);

    public class SMABars
    {
        public DateTime Date { get; set; }
        public double periodClosingPrice { get; set; }
    }

    public class SMAResult
    {
        public List<SMABars> SMABars { get; set; }

        public double SMAForPeriod { get; set; }
    }
}
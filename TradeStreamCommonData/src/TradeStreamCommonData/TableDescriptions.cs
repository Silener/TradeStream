namespace TradeStreamCommonData.TableDescriptions
{

    public static class TableDescriptions
    {
        public enum Tables
        {
            DataStreams = 1
        }

        public static readonly IDictionary<Tables, string> tableDictionary = new Dictionary<Tables, string>
        {
            { Tables.DataStreams, "DATA_STREAMS" }
        };

        public enum DataStreamsColumns
        {
            ID,
            RecordDate,
            SymbolType,
            Price
        }

        public static readonly IDictionary<DataStreamsColumns, string> dataStreamsColumns = new Dictionary<DataStreamsColumns, string>
        {
            { DataStreamsColumns.ID, "ID" },
            { DataStreamsColumns.RecordDate, "RECORD_DATE"  },
            { DataStreamsColumns.SymbolType, "SYMBOL_TYPE" },
            { DataStreamsColumns.Price, "PRICE" }
        };
    };
}
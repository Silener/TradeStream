using TradeStreamCommonData.TableDescriptions;

namespace TradeStreamCommonData.DateResolver
{
    public static class DateResolver
    {
        public static String GetSMABarsQuery(int dataPoints, string period, int symbolType, DateTime dateFrom)
        {
            string dataStreamsTable = TableDescriptions.TableDescriptions.tableDictionary[TableDescriptions.TableDescriptions.Tables.DataStreams];
            string IDColumn = TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.ID];
            string dateColumn = TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.RecordDate];
            string symbolTypeColumn = TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.SymbolType];
            string priceColumn = TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.Price];

            string SMAPeriodQuery = GetSMAPeriodQuery(period);

            String SMAQuery = $@"SELECT TOP {dataPoints} MAX({dateColumn}) AS DATE, MAX({IDColumn}) AS ID
                                 FROM {dataStreamsTable} WITH(NOLOCK)
                                 WHERE {symbolTypeColumn} = {symbolType}
                                 AND {dateColumn} <= '{dateFrom.ToString("yyyy\'-'MM'-'dd'T'HH':'mm':'ss")}'
                                 GROUP BY DATEPART(YEAR, {dateColumn})
	                            , DATEPART(MONTH, {dateColumn})
                                {SMAPeriodQuery}
	                            ORDER BY DATE DESC";

            return SMAQuery;
        }
        private static String GetSMAPeriodQuery(string period)
        {
            string dateColumn = TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.RecordDate];
            string SMAPeriodQuery = "";

            switch (period)
            {
                case "1w":
                    SMAPeriodQuery = $", DATEPART(WEEK, {dateColumn})";
                    break;
                case "1d":
                    SMAPeriodQuery = @$", DATEPART(WEEK, {dateColumn})
	                                    , DATEPART(DAY, {dateColumn})";
                    break;
                case "30m":
                    SMAPeriodQuery = @$", DATEPART(WEEK, {dateColumn})
	                                    , DATEPART(DAY, {dateColumn})
                                        , DATEPART(HOUR, {dateColumn})
	                                    , DATEPART(MINUTE, {dateColumn}) / 30";
                    break;
                case "5m":
                    SMAPeriodQuery = @$", DATEPART(WEEK, {dateColumn})
	                                    , DATEPART(DAY, {dateColumn})
                                        , DATEPART(HOUR, {dateColumn})
	                                    , DATEPART(MINUTE, {dateColumn}) / 5";
                    break;
                case "1m":
                    SMAPeriodQuery = @$", DATEPART(WEEK, {dateColumn})
	                                    , DATEPART(DAY, {dateColumn})
                                        , DATEPART(HOUR, {dateColumn})
	                                    , DATEPART(MINUTE, {dateColumn})";
                    break;
            }

            return SMAPeriodQuery;
        }
    }
}
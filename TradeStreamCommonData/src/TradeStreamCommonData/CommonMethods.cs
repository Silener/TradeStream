using System.Globalization;
using Microsoft.Data.SqlClient;
using TradeStreamCommonData.DatabaseConnector;
using TradeStreamCommonData.DataObjects;
using TradeStreamCommonData.Enums;

namespace TradeStreamCommonData.CommonMethods
{
    public class CommonMethods
    {
        public double GetAveragePriceFor24Hours(string symbol)
        {
            int symbolType = (int)SymbolTypesDictionary.symbolsDictionary[symbol];

            // Take last date
            DateTime lastDay = DateTime.UtcNow.AddDays(-1);

            // Build the query
            String sqlQuery = $@"SELECT * FROM {TableDescriptions.TableDescriptions.tableDictionary[TableDescriptions.TableDescriptions.Tables.DataStreams]}
                                 WITH(NOLOCK) WHERE {TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.RecordDate]}
                                 > '{lastDay.ToString("yyyy\'-'MM'-'dd'T'HH':'mm':'ss")}'
                                 AND {TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.SymbolType]} = {symbolType}";

            double averagePrice = 0.0;
            int recordCounter = 0;

            // Iterating data to sum the average price
            using (SqlCommand command = new SqlCommand(sqlQuery, DatabaseConnector.DatabaseConnector.GetInstance().GetConnection()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        recordCounter++;
                        averagePrice += reader.GetDouble((int)TableDescriptions.TableDescriptions.DataStreamsColumns.Price);
                    }
                }
            }

            return averagePrice / recordCounter;
        }

        public SMAResult GetSMAForPeriod(string symbol, int numberOfDataPoints, string timePeriod, string? startDateTime = "")
        {
            List<SMABars> smaList = new List<SMABars>();
            double dSMAValue = 0.0;
            int symbolType = (int)SymbolTypesDictionary.symbolsDictionary[symbol];

            DateTime dateFrom;
            if (startDateTime.Length <= 0)
            {
                dateFrom = DateTime.UtcNow;
            }
            else
            {
                DateTime passedDate = DateTime.ParseExact(startDateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime fullDate = new DateTime(passedDate.Year, passedDate.Month, passedDate.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second);
                dateFrom = fullDate;
            }

            // Building the SMA query depending on the period passed
            string SMABarsQuery = DateResolver.DateResolver.GetSMABarsQuery(numberOfDataPoints, timePeriod, symbolType, dateFrom);

            string dataStreamsTable = TableDescriptions.TableDescriptions.tableDictionary[TableDescriptions.TableDescriptions.Tables.DataStreams];
            string IDColumn = TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.ID];
            string dateColumn = TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.RecordDate];
            string symbolTypeColumn = TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.SymbolType];
            string priceColumn = TableDescriptions.TableDescriptions.dataStreamsColumns[TableDescriptions.TableDescriptions.DataStreamsColumns.Price];

            // Constructing the whole query
            String sqlQuery = $@"SELECT {dataStreamsTable}.{dateColumn}, {priceColumn} FROM {dataStreamsTable} WITH(NOLOCK)
                            INNER JOIN
                            (
                                {SMABarsQuery}
                            ) AS FILTERED_SMA_STREAM
	                            ON {dataStreamsTable}.{IDColumn} = FILTERED_SMA_STREAM.{IDColumn}
                            ORDER BY {dateColumn} DESC";

            double averageClosingPrice = 0.0;
            int recordCounter = 0;

            // Filling the list
            using (SqlCommand command = new SqlCommand(sqlQuery, DatabaseConnector.DatabaseConnector.GetInstance().GetConnection()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SMABars smaBar = new SMABars();
                        smaBar.Date = reader.GetDateTime(0);
                        smaBar.periodClosingPrice = reader.GetDouble(1);
                        averageClosingPrice += smaBar.periodClosingPrice;
                        recordCounter++;
                        smaList.Add(smaBar);
                    }
                }
            }

            dSMAValue = averageClosingPrice == 0.0 ? 0.0 : averageClosingPrice / recordCounter;

            SMAResult smaResult = new SMAResult();
            smaResult.SMABars = smaList;
            smaResult.SMAForPeriod = dSMAValue;

            return smaResult;
        }
    }
}
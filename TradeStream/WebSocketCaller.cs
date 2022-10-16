using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using TradeStreamCommon.Enums;
using TradeStream.WebSocketObjects;
using TradeStreamCommon.DatabaseConnector;

namespace TradeStream.WebSocketCaller
{
    public class WebSocketCaller
    {
        // Symbol provided
        private string symbol { get; set; }

        // Symbol as enum value
        private SymbolTypes symbolType { get; set; }

        public WebSocketCaller(string symbol, SymbolTypes symbolType)
        {
            this.symbol = symbol;
            this.symbolType = symbolType;
        }
        public async Task<bool> GetWebSocketData()
        {
            try
            {
                // Connect to the stream
                ClientWebSocket webSocket = new ClientWebSocket();
                CancellationToken stopToken = new CancellationToken();
                await webSocket.ConnectAsync(new Uri($"wss://stream.binance.com:9443/ws/{symbol}@kline_1s"), stopToken);

                // While state is open, read from stream
                while (webSocket.State == WebSocketState.Open)
                {
                    // Declare bytes for reading
                    ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);

                    // Receive data async
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(bytesReceived, CancellationToken.None);

                    // Convert to JSON
                    string json = Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);

                    // Deserialize to KlineStreamData
                    KlineStreamData klineStreamData = JsonSerializer.Deserialize<KlineStreamData>(json);

                    if (klineStreamData == null)
                        continue;

                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(klineStreamData.eventEpochTIme);

                    // Start database adding
                    SqlCommand sqlCommand;
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                    String sqlQuery = @$"INSERT INTO DATA_STREAMS VALUES('{dateTimeOffset.DateTime.ToString("yyyy\'-'MM'-'dd'T'HH':'mm':'ss")}',
                                         {(int)symbolType}, {klineStreamData.klineDetailedData.closePrice} )";
                    sqlCommand = new SqlCommand(sqlQuery, DatabaseConnector.GetInstance().GetConnection());

                    sqlAdapter.InsertCommand = new SqlCommand(sqlQuery, DatabaseConnector.GetInstance().GetConnection());
                    await sqlAdapter.InsertCommand.ExecuteNonQueryAsync();

                    sqlCommand.Dispose();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.ToString());
            }

            return true;
        }
    }
}
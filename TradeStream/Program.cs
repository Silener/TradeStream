using System.Net;
using TradeStreamCommon.DatabaseConnector;
using TradeStreamCommon.Enums;
using TradeStream.WebSocketCaller;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("---TradeStream Server---\n");

        // Start the connection to the database
        Console.WriteLine("Initializing database connection...\n");
        DatabaseConnector.GetInstance();
        Console.WriteLine("Database connection initialized.\n");

        // Start first stream
        Console.WriteLine("Starting btcusdt stream reading...\n");
        WebSocketCaller webSocketCallerBTCUSDT = new WebSocketCaller("btcusdt", SymbolTypes.symbolTypeBTCUSDT);

        Task.Run(() => webSocketCallerBTCUSDT.GetWebSocketData());
        Console.WriteLine("btcusdt btcusdt stream reading started.\n");

        // Start second stream
        Console.WriteLine("Starting adausdt stream reading...\n");
        WebSocketCaller webSocketCallerADAUSDT = new WebSocketCaller("adausdt", SymbolTypes.symbolTypeADAUSDT);

        Task.Run(() => webSocketCallerADAUSDT.GetWebSocketData());
        Console.WriteLine("btcusdt adausdt stream reading started.\n");

        // Start third
        Console.WriteLine("Starting ethusdt stream reading...\n");
        WebSocketCaller webSocketCallerETHUSDT = new WebSocketCaller("ethusdt", SymbolTypes.symbolTypeETHUSDT);

        Task.Run(() => webSocketCallerETHUSDT.GetWebSocketData());
        Console.WriteLine("btcusdt ethusdt stream reading started.\n");

        Console.WriteLine("Type \"Q\" to quit the server\n");

        for (; ; )
        {
            var choice = Console.ReadLine();

            // If symbol typed is "Q" or "q", quit the server
            if (choice != null && (Convert.ToString(choice).CompareTo("Q") == 0 || Convert.ToString(choice).CompareTo("q") == 0))
            {
                DatabaseConnector.GetInstance().Disconnect();
                System.Environment.Exit(1);
            }

        }
    }
}
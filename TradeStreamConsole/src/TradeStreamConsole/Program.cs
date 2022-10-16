using TradeStreamCommon.DatabaseConnector;
using TradeStreamCommonData.DataObjects;
using TradeStreamConsole;

Console.WriteLine("---TradeStream Console---\n");

// Start the connection to the database
Console.WriteLine("Initializing database connection...\n");
DatabaseConnector.GetInstance();
Console.WriteLine("Database connection initialized.\n");

Console.WriteLine("Type the desired request:\n");
Console.WriteLine("1. Returns the last 24 hours average price for a crypto. Example request: \"24h btcusdt\".\n");
Console.WriteLine("2. Returns SMA for the selected crypto, data points, period and optional date. Example request: \"sma btcusdt 100 30m 2022-10-10\"\n");
Console.WriteLine("Type \"Q\" to quit the console\n");

for (; ; )
{
    var choice = Console.ReadLine();

    if (choice == null)
        continue;

    // If symbol typed is "Q" or "q", quit the server
    if (choice != null && (Convert.ToString(choice).CompareTo("Q") == 0 || Convert.ToString(choice).CompareTo("q") == 0))
    {
        DatabaseConnector.GetInstance().Disconnect();
        System.Environment.Exit(1);
    }// if

    string[] request = choice.Split(' ');

    if (request.Count() <= 1)
        continue;

    ChoiceResolver choiceResolver = new ChoiceResolver();

    switch (request[0])
    {
        case "24h":
            {
                double average = 0.0;
                if (!choiceResolver.GetAverage(request[1], out average))
                {
                    Console.WriteLine("Symbol not found.\n");
                    break;
                }

                Console.WriteLine($"Last 24h average price - {average}.\n");

                break;
            }
        case "sma":
            {
                if (request.Count() < 4)
                {
                    Console.WriteLine("Bad request parameters.\n");
                    break;
                }

                int numberOfDataPoints = 0;
                if (!Int32.TryParse(request[2], out numberOfDataPoints))
                {
                    Console.WriteLine("Bad data points parameter.\n");
                    break;
                }
                SMAResult smaResult = new SMAResult();

                DateTime dateTryParse;
                string parsedDate = "";

                if (request.Count() == 5)
                {
                    if (!DateTime.TryParse(request[4], out dateTryParse))
                    {
                        Console.WriteLine("Bad date parameter.\n");
                        break;
                    }
                    else
                        parsedDate = dateTryParse.ToString("yyyy-MM-dd");
                }

                if (!choiceResolver.GetSMAForPeriod(request[1], numberOfDataPoints, request[3], out smaResult, parsedDate))
                {
                    Console.WriteLine("Bad request parameters.\n");
                    break;
                }

                string json = System.Text.Json.JsonSerializer.Serialize(smaResult);
                Console.WriteLine($"{json}\n");
                break;
            }
    }
}
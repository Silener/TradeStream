using System.Globalization;
using System.Net;
using TradeStreamCommonData.DatabaseConnector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TradeStreamCommonData.Enums;
using TradeStreamCommonData.DataObjects;
using TradeStreamCommonData.TableDescriptions;
using TradeStreamCommonData.DateResolver;
using TradeStreamCommonData.CommonMethods;

namespace TradeStreamAPI.Controllers;

[ApiController]
[Route("/api/")]
public class TradeController : ControllerBase
{
    private readonly ILogger<TradeController> _logger;

    public TradeController(ILogger<TradeController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{symbol}/24hAvgPrice")]
    public ActionResult<IEnumerable<double>> GetAvg(string symbol)
    {
        if (!SymbolTypesDictionary.symbolsDictionary.ContainsKey(symbol))
            return NotFound(symbol);

        CommonMethods commonMethods = new CommonMethods();

        double averagePrice = commonMethods.GetAveragePriceFor24Hours(symbol);

        return Ok(averagePrice);

    }

    [HttpGet]
    [Route("{symbol}/SimpleMovingAverage/{numberOfDataPoints}/{timePeriod}/{startDateTime?}")]
    public ActionResult<IEnumerable<List<DataStreams>>> GetSMA(string symbol, int numberOfDataPoints, string timePeriod, string? startDateTime = "")
    {
        SMAResult smaResult = new SMAResult();
        double dSMAValue = 0.0;

        try
        {
            // Making validations
            if (!SymbolTypesDictionary.symbolsDictionary.ContainsKey(symbol))
                return NotFound(symbol);

            if (!SupportedPeriodsDictionary.supportedPeriodsDictionary.ContainsKey(timePeriod))
                return NotFound(timePeriod);

            if (startDateTime.Length > 0)
            {
                DateTime dateValidate;
                if (!DateTime.TryParse(startDateTime, out dateValidate))
                    return BadRequest(startDateTime);
            }

            if (numberOfDataPoints <= 0)
                return BadRequest(numberOfDataPoints);

            CommonMethods commonMethods = new CommonMethods();

            smaResult = commonMethods.GetSMAForPeriod(symbol, numberOfDataPoints, timePeriod, startDateTime);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }

        return Ok(smaResult);

    }
}
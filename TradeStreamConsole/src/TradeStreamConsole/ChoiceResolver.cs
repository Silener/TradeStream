using TradeStreamCommonData.CommonMethods;
using TradeStreamCommonData.DataObjects;
using TradeStreamCommonData.Enums;

namespace TradeStreamConsole
{
    public class ChoiceResolver
    {
        public bool GetAverage(string symbol, out double average)
        {
            average = 0.0;

            if (!SymbolTypesDictionary.symbolsDictionary.ContainsKey(symbol))
                return false;

            CommonMethods commonMethods = new CommonMethods();

            double averagePrice = commonMethods.GetAveragePriceFor24Hours(symbol);

            average = averagePrice;

            return true;
        }

        public bool GetSMAForPeriod(string symbol, int numberOfDataPoints, string timePeriod, out SMAResult smaResult, string? startDateTime = "")
        {
            double dSMAValue = 0.0;
            smaResult = new SMAResult();

            try
            {
                // Making validations
                if (!SymbolTypesDictionary.symbolsDictionary.ContainsKey(symbol))
                    return false;

                if (!SupportedPeriodsDictionary.supportedPeriodsDictionary.ContainsKey(timePeriod))
                    return false;

                if (startDateTime.Length > 0)
                {
                    DateTime dateValidate;
                    if (!DateTime.TryParse(startDateTime, out dateValidate))
                        return false;
                }

                if (numberOfDataPoints <= 0)
                    return false;

                CommonMethods commonMethods = new CommonMethods();

                smaResult = commonMethods.GetSMAForPeriod(symbol, numberOfDataPoints, timePeriod, startDateTime);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
    }
}
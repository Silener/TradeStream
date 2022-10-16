namespace TradeStreamCommonData.Enums
{
    public enum SymbolTypes
    {
        symbolTypeBTCUSDT = 1,
        symbolTypeADAUSDT,
        symbolTypeETHUSDT
    }
    public class SymbolTypesDictionary
    {
        public static IDictionary<string, SymbolTypes> symbolsDictionary = new Dictionary<string, SymbolTypes>
        {
            { "btcusdt", SymbolTypes.symbolTypeBTCUSDT },
            { "adausdt", SymbolTypes.symbolTypeADAUSDT },
            { "ethusdt", SymbolTypes.symbolTypeETHUSDT  }
        };
    }

    public enum SupportedPeriods
    {
        Week = 1,
        Day,
        ThirtyMinutes,
        FiveMinutes,
        Minute
    }
    public class SupportedPeriodsDictionary
    {
        public static IDictionary<string, SupportedPeriods> supportedPeriodsDictionary = new Dictionary<string, SupportedPeriods>
        {
            { "1w", SupportedPeriods.Week },
            { "1d", SupportedPeriods.Day },
            { "30m", SupportedPeriods.ThirtyMinutes  },
            { "5m", SupportedPeriods.FiveMinutes  },
            { "1m", SupportedPeriods.Minute  }
        };
    }

}
namespace school_major_project.GlobalServices
{
    public sealed class ExchangeCurrencyService
    {
        private static readonly Lazy<ExchangeCurrencyService> _instance =
      new Lazy<ExchangeCurrencyService>(() => new ExchangeCurrencyService());

        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://api.exchangerate-api.com/v4/latest/USD";
        private const string FromCurrency = "VND";
        private const string ToCurrency = "USD";

        private ExchangeCurrencyService() { }

        public static ExchangeCurrencyService Instance => _instance.Value;


        public static async Task<decimal> ConvertCurrency(long amount)
        {

            using (var httpClient = new HttpClient())
            {
                try
                {
                    ExchangeRates exchangeRates = await httpClient.GetFromJsonAsync<ExchangeRates>(ApiUrl);

                    if (exchangeRates == null || !exchangeRates.Rates.ContainsKey(ToCurrency))
                    {
                        throw new Exception("Invalid currency code");
                    }

                    decimal rate = 1 / exchangeRates.Rates[FromCurrency];
                    return amount * rate;
                }
                catch (Exception e)
                {
                    // Log the error
                    throw new Exception("Error fetching exchange rates", e);
                }
            }
        }

        private class ExchangeRates
        {
            public Dictionary<string, decimal> Rates { get; set; }
        }
    }
}

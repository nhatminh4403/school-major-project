using Newtonsoft.Json;
using school_major_project.DataAccess;

namespace school_major_project.GlobalServices
{
    public class TokenGoogle
    {
        //[JsonProperty(PropertyName = "error_description")]
        public string error_description { get; set; }
        //[JsonProperty(PropertyName = "name")]
        public string name { get; set; }
        //[JsonProperty(PropertyName = "email")]
        public string email { get; set; }
        public string phone { get; set; }
    }
    public class GoogleQuery
    {
        private static HttpClient httpClient = new HttpClient();
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public GoogleQuery(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _clientId = configuration["Google:ClientId"];
            _clientSecret = configuration["Google:ClientSecret"];
        }

        public string GetClientId()
        {
            // Return the value stored during construction
            return _clientId;
        }


        public static async Task<TokenGoogle> VerifyTokenGoogle(string token)
        {
            TokenGoogle json = new();
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token}"))
            {
                requestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var t = JsonConvert.DeserializeObject<TokenGoogle>(await response.Content.ReadAsStringAsync());
                    if (t.error_description == null)
                    {
                        json.name = t.name;
                        json.email = t.email;
                        json.phone = t.phone;
                    }
                    else json.error_description = t.error_description;
                }
            }
            return json;
        }
    }
}

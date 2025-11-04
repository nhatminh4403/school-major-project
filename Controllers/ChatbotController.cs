using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using school_major_project.DataAccess;
using System.Text.Json;


namespace school_major_project.Controllers
{
    [AllowAnonymous]
    public class ChatbotController : Controller
    {
        private readonly string _googleProjectId;
        private readonly string _languageCode;
        private readonly SessionsClient _sessionsClient;
        private readonly ApplicationDbContext _applicationDbContext;

        public ChatbotController(IConfiguration configuration, ApplicationDbContext context)
        {
            _applicationDbContext = context;


            _googleProjectId = configuration["Dialogflow:ProjectId"];
            _languageCode = configuration["Dialogflow:LanguageCode"];
            string relativeCredentialsPath = configuration["Dialogflow:CredentialsPath"];


            if (string.IsNullOrEmpty(_googleProjectId))
            {
                throw new ArgumentNullException(nameof(_googleProjectId),
                    "Dialogflow Project ID ('Dialogflow:ProjectId') is not configured in appsettings.json.");
            }
            if (string.IsNullOrEmpty(_languageCode))
            {

                _languageCode = "en-US";
            }
            if (string.IsNullOrEmpty(relativeCredentialsPath))
            {
                throw new ArgumentNullException(nameof(relativeCredentialsPath),
                    "Dialogflow Credentials Path ('Dialogflow:CredentialsPath') is not configured in appsettings.json.");
            }


            string absoluteCredentialPath = Path.Combine(Directory.GetCurrentDirectory(), relativeCredentialsPath);

            if (!System.IO.File.Exists(absoluteCredentialPath))
            {
                throw new FileNotFoundException($"Google Cloud credentials file not found at resolved path: '{absoluteCredentialPath}'. " +
                                                "Check 'Dialogflow:CredentialsPath' in appsettings.json, ensure the file exists, " +
                                                "and set 'Copy to Output Directory' to 'Copy if newer' or 'Copy always' in Visual Studio properties for the file.");
            }

            try
            {
                var credentials = GoogleCredential.FromFile(absoluteCredentialPath)
                                      .CreateScoped(SessionsClient.DefaultScopes);
                _sessionsClient = new SessionsClientBuilder { Credential = credentials }.Build();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize Dialogflow client. Check credentials and configuration.", ex);
            }
        }


        [HttpGet]
        public IActionResult Chat()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.SessionId))
            {
                return BadRequest(new { replies = new List<string> { "Lỗi: SessionId là bắt buộc." } });
            }
            if (string.IsNullOrWhiteSpace(request.Message) && string.IsNullOrWhiteSpace(request.EventName))
            {
                return BadRequest(new { replies = new List<string> { "Lỗi: Message hoặc EventName là bắt buộc." } });
            }

            try
            {
                var sessionName = new SessionName(_googleProjectId, request.SessionId);
                QueryInput queryInput = !string.IsNullOrWhiteSpace(request.EventName)
                    ? new QueryInput { Event = new EventInput { Name = request.EventName, LanguageCode = _languageCode } }
                    : new QueryInput { Text = new TextInput { Text = request.Message, LanguageCode = _languageCode } };

                var response = await _sessionsClient.DetectIntentAsync(new DetectIntentRequest
                {
                    SessionAsSessionName = sessionName,
                    QueryInput = queryInput
                });

                var queryResult = response.QueryResult;
                var botReplies = new List<string>();
                JsonElement? payload = null; // Variable to hold our payload

                if (queryResult?.FulfillmentMessages != null)
                {
                    foreach (var msg in queryResult.FulfillmentMessages)
                    {
                        if (msg.MessageCase == Intent.Types.Message.MessageOneofCase.Text)
                        {
                            botReplies.AddRange(msg.Text.Text_.Where(t => !string.IsNullOrWhiteSpace(t)));
                        }

                        // --- THIS IS THE NEW CODE ---
                        if (msg.MessageCase == Intent.Types.Message.MessageOneofCase.Payload)
                        {
                            string payloadJson = JsonFormatter.Default.Format(msg.Payload);
                            payload = JsonSerializer.Deserialize<JsonElement>(payloadJson);
                        }
                    }
                }

                if (!botReplies.Any() && payload == null && !string.IsNullOrWhiteSpace(queryResult?.FulfillmentText))
                {
                    botReplies.Add(queryResult.FulfillmentText);
                }

                // Return both text replies AND the payload
                return Ok(new { replies = botReplies, payload });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new { replies = new List<string> { "Xin lỗi, tôi gặp lỗi nội bộ khi xử lý yêu cầu." } });
            }
        }
        public class ChatRequest
        {
            public string Message { get; set; }
            public string EventName { get; set; }
            public string SessionId { get; set; }
        }
    }
}
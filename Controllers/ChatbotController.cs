using Google.Apis.Auth.OAuth2; // Needed for GoogleCredential
using Google.Cloud.Dialogflow.V2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// Assuming you have this using statement for your DbContext if needed
// using school_major_project.Data; 
using school_major_project.DataAccess; // Needed for Task


namespace school_major_project.Controllers
{
    [AllowAnonymous]
    public class ChatbotController : Controller
    {
        // Your existing Project ID and DbContext injection
        private const string _googleProjectId = "movie-vaqw";
        private readonly ApplicationDbContext _applicationDbContext;

        public ChatbotController(ApplicationDbContext context)
        {
            _applicationDbContext = context;
        }   

        [HttpGet]
        public IActionResult Chat()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request) // Use the MODIFIED ChatRequest below
        {
            // --- MODIFIED Validation: Check SessionId and (Message OR EventName) ---
            if (request == null || string.IsNullOrWhiteSpace(request.SessionId))
            {
                // Return error in the new format
                return BadRequest(new { replies = new List<string> { "Lỗi: SessionId là bắt buộc." } });
            }
            if (string.IsNullOrWhiteSpace(request.Message) && string.IsNullOrWhiteSpace(request.EventName))
            {
                // Return error in the new format
                return BadRequest(new { replies = new List<string> { "Lỗi: Message hoặc EventName là bắt buộc." } });
            }
            // --- END MODIFIED Validation ---

            try
            {
                // --- Keep your existing credential loading ---
                string _googleCredentialPath = Path.Combine(Directory.GetCurrentDirectory(), "credentials", "movie-vaqw-ebd6910c61fd.json");

                if (string.IsNullOrEmpty(_googleProjectId)) // This check is slightly redundant as it's a const
                {
                    throw new ArgumentNullException(nameof(_googleProjectId), "Google Cloud Project ID is not configured.");
                }
                if (string.IsNullOrEmpty(_googleCredentialPath) || !System.IO.File.Exists(_googleCredentialPath))
                {
                    // Ensure 'Copy to Output Directory' is set for the credentials file in VS.
                    throw new FileNotFoundException($"Google Cloud credentials file not found at: '{_googleCredentialPath}'.");
                }

                var credentials = GoogleCredential.FromFile(_googleCredentialPath)
                                   .CreateScoped(SessionsClient.DefaultScopes);
                var sessionsClient = new SessionsClientBuilder
                {
                    Credential = credentials
                }.Build();
                // The System.Environment calls are generally not needed when passing credentials directly
                // System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _googleCredentialPath);
                // -------------------------------------------------

                var sessionName = new SessionName(_googleProjectId, request.SessionId);

                // --- *** MODIFIED: Create QueryInput based on Event or Text *** ---
                QueryInput queryInput;
                if (!string.IsNullOrWhiteSpace(request.EventName))
                {
                    // Create EventInput if EventName is provided
                    Console.WriteLine($"Processing EVENT: {request.EventName} for session: {request.SessionId}");
                    queryInput = new QueryInput
                    {
                        Event = new EventInput
                        {
                            Name = request.EventName,
                            LanguageCode = "vi"
                        }
                    };
                }
                else
                {
                    // Otherwise, create TextInput using the Message
                    Console.WriteLine($"Processing TEXT: '{request.Message}' for session: {request.SessionId}");
                    queryInput = new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = request.Message,
                            LanguageCode = "vi" // Assuming Vietnamese
                        }
                    };
                }
                // --- *** END MODIFIED QueryInput *** ---

                var detectIntentRequest = new DetectIntentRequest
                {
                    SessionAsSessionName = sessionName,
                    QueryInput = queryInput
                };

                DetectIntentResponse response = await sessionsClient.DetectIntentAsync(detectIntentRequest);
                Console.WriteLine($"DetectIntent successful. Intent: {response.QueryResult?.Intent?.DisplayName}");

                // --- *** MODIFIED: Extract Multiple Replies *** ---
                List<string> botReplies = new List<string>();
                // Use null-conditional operator ?. to safely access properties
                if (response.QueryResult?.FulfillmentMessages != null && response.QueryResult.FulfillmentMessages.Any())
                {
                    Console.WriteLine($"Found {response.QueryResult.FulfillmentMessages.Count} fulfillment messages.");
                    foreach (var msg in response.QueryResult.FulfillmentMessages)
                    {
                        if (msg.MessageCase == Intent.Types.Message.MessageOneofCase.Text)
                        {
                            var texts = msg.Text.Text_.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                            if (texts.Any())
                            {
                                Console.WriteLine($"Adding text message(s): {string.Join(" | ", texts)}");
                                botReplies.AddRange(texts);
                            }
                        }
                        // TODO: Handle other message types (cards, quick replies) if needed
                    }
                }
                else
                {
                    Console.WriteLine("No structured FulfillmentMessages found.");
                }

                // Fallback to FulfillmentText if no structured messages were added
                if (!botReplies.Any() && !string.IsNullOrWhiteSpace(response.QueryResult?.FulfillmentText))
                {
                    Console.WriteLine($"Using FulfillmentText fallback: '{response.QueryResult.FulfillmentText}'");
                    botReplies.Add(response.QueryResult.FulfillmentText);
                }
                // --- *** END MODIFIED Reply Extraction *** ---


                // --- *** MODIFIED: Return Replies Array *** ---
                Console.WriteLine($"Returning {botReplies.Count} replies.");
                return Ok(new { replies = botReplies });
                // --- *** END MODIFIED Return *** ---

            }
            catch (Exception ex)
            {
                // --- Keep your existing logging ---
                Console.Error.WriteLine("------ DIALOGFLOW ERROR ------");
                Console.Error.WriteLine($"Timestamp: {DateTime.UtcNow}");
                Console.Error.WriteLine($"SessionId: {request?.SessionId}"); // Add SessionId context
                Console.Error.WriteLine($"Exception Type: {ex.GetType().FullName}");
                Console.Error.WriteLine($"Message: {ex.Message}");
                Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null) { /* Log inner */ }
                Console.Error.WriteLine("------ END DIALOGFLOW ERROR ------");
                // --- END Logging ---

                // --- MODIFIED: Return error in the new format ---
                return StatusCode(500, new { replies = new List<string> { "Xin lỗi, tôi gặp lỗi nội bộ khi xử lý yêu cầu." } });
                // --- END MODIFIED Error Return ---
            }
        }

        // --- MODIFIED ChatRequest class ---
        public class ChatRequest
        {
            public string Message { get; set; }     // User's text input
            public string EventName { get; set; }   // Event name input (e.g., "WELCOME")
            public string SessionId { get; set; }   // Conversation session ID (Required)
        }
        // --- END MODIFIED ChatRequest ---
    }
}
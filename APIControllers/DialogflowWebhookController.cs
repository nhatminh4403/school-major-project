using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.Extensions;
using school_major_project.Interfaces;
using System.Text.Json;

namespace school_major_project.APIControllers
{
    [Route("api/dialogflow")]
    [ApiController]
    public class DialogflowWebhookController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<DialogflowWebhookController> _logger;
        private static readonly JsonParser JsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
        private readonly ApplicationDbContext _applicationDbContext;

        public DialogflowWebhookController(ICategoryRepository categoryRepository, ILogger<DialogflowWebhookController> logger, ApplicationDbContext applicationDbContext)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { text = "success" });
        }

        [HttpPost]
        public async Task<ContentResult> HandleWebhook()
        {
            WebhookRequest request;
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                _logger.LogInformation("Webhook request body: {Body}", body);
                request = JsonParser.Parse<WebhookRequest>(body);
            }

            var intentName = request.QueryResult.Intent.DisplayName;
            _logger.LogInformation("Intent received: {IntentName}", intentName);

            WebhookResponse response = new WebhookResponse();

            if (intentName == "suggest category")
            {
                // Get categories from database
                var categories = _applicationDbContext.Categories
                    .Select(c => c.CategoryDescription)
                    .Take(5)
                    .ToList();

                _logger.LogInformation("Found {Count} categories", categories.Count);

                if (categories.Any())
                {
                    // Add a text message first
                    response.FulfillmentMessages.Add(new Intent.Types.Message
                    {
                        Text = new Intent.Types.Message.Types.Text
                        {
                            Text_ = { "Dưới đây là các danh mục khóa học:" }
                        }
                    });

                    // Create chip options array
                    var chipOptions = categories.Select(name => new
                    {
                        text = name
                    }).ToArray();

                    // Create the rich content structure correctly
                    var richContentStructure = new
                    {
                        richContent = new[]
                        {
                            new object[]
                            {
                                new
                                {
                                    type = "info",
                                    title = "Chọn danh mục khóa học",
                                    subtitle = "Nhấp vào một trong các tùy chọn bên dưới"
                                },
                                new
                                {
                                    type = "chips",
                                    options = chipOptions
                                }
                            }
                        }
                    };

                    // Serialize to JSON
                    var jsonPayload = JsonSerializer.Serialize(richContentStructure);
                    _logger.LogInformation("Payload JSON: {Json}", jsonPayload);

                    // Parse to Protobuf Struct
                    var payloadStruct = Struct.Parser.ParseJson(jsonPayload);

                    // Add payload message
                    response.FulfillmentMessages.Add(new Intent.Types.Message
                    {
                        Payload = payloadStruct
                    });
                }
                else
                {
                    response.FulfillmentText = "Xin lỗi, hiện tại không có danh mục nào.";
                }
            }
            else
            {
                response.FulfillmentText = "Xin lỗi, tôi không hiểu yêu cầu của bạn.";
            }

            // Use JsonFormatter to convert response to JSON properly
            var formatter = new JsonFormatter(JsonFormatter.Settings.Default);
            var jsonResponse = formatter.Format(response);

            _logger.LogInformation("Webhook response: {Response}", jsonResponse);

            return new ContentResult
            {
                Content = jsonResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
        }
    }
}
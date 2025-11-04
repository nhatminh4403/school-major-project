using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using school_major_project.Extensions;
using school_major_project.Interfaces;

namespace school_major_project.APIControllers
{
    [Route("api/dialogflow")]
    [ApiController]
    public class DialogflowWebhookController : ControllerBase
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<DialogflowWebhookController> _logger;

        public DialogflowWebhookController(ICategoryRepository categoryRepository, ILogger<DialogflowWebhookController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { text = "success" });
        }
        [HttpPost]
        public async Task<ContentResult> HandleWebhook()
        {
            // Official way to parse the JSON request from Dialogflow
            var parser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
            WebhookRequest request;
            using (var reader = new StreamReader(Request.Body))
            {
                request = parser.Parse<WebhookRequest>(await reader.ReadToEndAsync());
            }

            // Create a response object
            var response = new WebhookResponse();

            // Check which intent was triggered
            if (request.QueryResult.Intent.DisplayName == "suggest category")
            {
                // Query the database for the top 5 movie categories
                var categories = await _categoryRepository.GetAllAsync();
                var getRandomCategoriesForAnswer = categories.Shuffle().Take(5);
                // Create the JSON payload structure dynamically
                var payload = new Struct();

                // 1. Create the 'options' array for the chips
                var chipsOptionsList = new ListValue();
                foreach (var category in getRandomCategoriesForAnswer)
                {
                    var chip = new Struct();
                    chip.Fields.Add("text", Value.ForString(category.CategoryDescription));
                    chipsOptionsList.Values.Add(Value.ForStruct(chip));
                }

                // 2. Create the 'chips' object
                var chipsStruct = new Struct();
                chipsStruct.Fields.Add("type", Value.ForString("chips"));
                //chipsStruct.Fields.Add("options", Value.ForList(chipsOptionsList));

                // 3. Create the 'info' object for the title

                // With this line:
                chipsStruct.Fields.Add("options", Value.ForList(chipsOptionsList.Values.ToArray()));
                var infoStruct = new Struct();
                infoStruct.Fields.Add("type", Value.ForString("info"));
                infoStruct.Fields.Add("title", Value.ForString("Bạn muốn xem thể loại phim nào?"));
                infoStruct.Fields.Add("subtitle", Value.ForString("Chọn một thể loại từ CSDL của chúng tôi:"));

                // 4. Create the 'richContent' array and add the info and chips
                var richContentList = new ListValue();
                richContentList.Values.Add(Value.ForStruct(infoStruct));
                richContentList.Values.Add(Value.ForStruct(chipsStruct));

                // 5. Add 'richContent' to the main payload
                payload.Fields.Add("richContent", Value.ForList(richContentList.Values.ToArray()));

                // Add the fully constructed payload to the fulfillment message
                response.FulfillmentMessages.Add(new Intent.Types.Message
                {
                    Payload = payload
                });
            }
            else
            {
                // Handle other intents or provide a default response
                response.FulfillmentText = "Sorry, I am not sure how to handle that intent via webhook yet.";
            }

            // Convert the response to a JSON string and return it
            string responseJson = response.ToString();
            return new ContentResult
            {
                Content = responseJson,
                ContentType = "application/json"
            };
        }
    }
}

using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace WebTH.Areas.Admin.Controllers
{
    public class ChatBotAIController : Controller
    {
        // GET: Admin/ChatBotAI
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> AskAI(string message)
        {
            try
            {
                var apiKey = ConfigurationManager.AppSettings["OpenRouterKey"];
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Add("HTTP-Referer", "https://localhost:44326/"); // vẫn dùng localhost
                client.DefaultRequestHeaders.Add("X-Title", "WebTH Chatbot");

                var requestData = new
                {
                    //model = "mistralai/mistral-7b-instruct"
                    //model = "deepseek/deepseek-r1-0528-qwen3-8b:free"
                    //model = "deepseek/deepseek-r1-0528:free"
                    //model = "sarvamai/sarvam-m:free"
                    //model = "mistralai/devstral-small:free"
                    //model = "google/gemma-3n-e4b-it:free"
                    //model = "nousresearch/deephermes-3-mistral-24b-preview:free"
                    //model = "qwen/qwen3-30b-a3b:free"
                    //model = "nvidia/llama-3.1-nemotron-ultra-253b-v1:free"
                    model = "meta-llama/llama-4-maverick:free"
                   //model = "mistralai/mistral-small-3.1-24b-instruct:free"
                   //model = "google/gemma-3-27b-it:free"
                   //model = "google/gemini-2.0-flash-exp:free"
                   //model = "rekaai/reka-flash-3:free"


                   //model = "meta-llama/llama-3.2-1b-instruct"

                   /* model = "meta-llama/llama-3.3-8b-instruct:free"*/, // hoặc model khác như gpt-3.5-turbo nếu bạn muốn
                    messages = new[] {
                new { role = "user", content = message }
            }
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { reply = $"Lỗi từ OpenRouter: {response.StatusCode} - {responseString}" }, JsonRequestBehavior.AllowGet);
                }

                dynamic json = JsonConvert.DeserializeObject(responseString);
                string reply = json?.choices[0]?.message?.content;

                return Json(new { reply = reply ?? "(AI không trả lời gì cả.)" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { reply = $"Lỗi xử lý: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}
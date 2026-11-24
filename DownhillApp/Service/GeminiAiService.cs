using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Google.GenAI;

namespace DownhillApp.Service
{
    public class GeminiAiService : IGeminiAiService
    {
        private Client client;
        public GeminiAiService()
        {
            this.client = new Client(apiKey: API_Keys.API_Gemini);
        }

        public async Task<string> GetResponse(string promt)
        {
            /*
             * models:
             * gemini-flash-latest
             * gemini-3.5-flash-lite
             */
            var response = await client.Models.GenerateContentAsync(model: "gemini-3.5-flash-lite", contents: promt);
            return response.Candidates[0].Content.Parts[0].Text;
        }
    }
}

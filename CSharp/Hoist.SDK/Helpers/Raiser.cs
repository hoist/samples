using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Hoist.SDK.Helpers
{
    internal class Raiser
    {
        public async void RaiseEvent(string eventName, object payload)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.hoi.io");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Hoist", Configuration.ApiKey);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = await client.PostAsync("/event/"+eventName, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
            }

        }
    }
    }
}

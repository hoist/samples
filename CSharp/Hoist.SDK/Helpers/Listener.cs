using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Timers;

namespace Hoist.SDK.Helpers
{

    public class Listener
    {
        private string apiKey;
        private IList<string> includeEvents;
        private IList<PollingUrl> pollingUrls = new List<PollingUrl>();

        public event EventHandler<Events.HoistEvent> NewEvent;

        public Listener(string apiKey, IList<string> includeEvents = null)
        {
            this.apiKey = apiKey;
            this.includeEvents = includeEvents;
        }

        public void Start()
        {
            if (includeEvents != null && includeEvents.Count() > 0)
            {
                foreach (var eventName in includeEvents)
                {
                    var pollingUrl = new PollingUrl(apiKey, eventName);
                    pollingUrl.OnNewEvent += (object sender, Events.HoistEvent hoistEvent) => { NewEvent.Invoke(this, hoistEvent); };
                    pollingUrl.Start();
                    pollingUrls.Add(pollingUrl);
                }
            }
            else
            {
                var pollingUrl = new PollingUrl(apiKey);
                pollingUrl.OnNewEvent += (object sender, Events.HoistEvent hoistEvent) => { NewEvent.Invoke(this, hoistEvent); };
                pollingUrl.Start();
                pollingUrls.Add(pollingUrl);
            }
        }

        public void End()
        {
            foreach(var poller in pollingUrls)
            {
                poller.End();
            }
        }

        private class PollingUrl
        {
            readonly Timer Timer = new Timer();
            private string apiKey;
            private string eventName;
            private UriBuilder requestUri;
            string token = null;
            internal event EventHandler<Events.HoistEvent> OnNewEvent;

            public PollingUrl(string apiKey, string eventName = null)
            {
                this.apiKey = apiKey;
                this.eventName = eventName;
                var query = "?";
                if (this.eventName != null)
                {
                    query += "filterBy=eventName&filterValue=" + Uri.EscapeUriString(eventName);
                }
                requestUri = new UriBuilder("https://api.hoi.io/events");
                requestUri.Query = query;
            }

            internal void Start()
            {
                //start a timer
                Timer.AutoReset = true;
                Timer.Interval = 2000;
                //on timer elapsed, poll url
                Timer.Elapsed += new ElapsedEventHandler(Poll);
                Timer.Start();
            }
            internal void End()
            {
                Timer.Stop();
            }
            internal async void Poll(object sender, ElapsedEventArgs args)
            {
                //make a web request
                using (var client = new HttpClient())
                {
                    client.BaseAddress = requestUri.Uri;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Hoist", Configuration.ApiKey);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // New code:
                    HttpResponseMessage response = await client.GetAsync(requestUri.Uri.PathAndQuery);
                    if (response.IsSuccessStatusCode)
                    {
                        var events = await response.Content.ReadAsAsync<IEnumerable<Events.HoistEvent>>();
                        foreach (var ev in events)
                        {
                            //fire any returned events
                            OnNewEvent.Invoke(this, ev);
                        }
                    }
                }

            }
        }
    }
}

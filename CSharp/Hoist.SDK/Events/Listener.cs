using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hoist.SDK.Events
{
    
    public class Listener
    {
        private string apiKey;
        private IList<String> includeEvents;
        private IList<PollingUrl> pollingUrls = new List<PollingUrl>();

        public event EventHandler<HoistEvent> NewEvent;
        
        public Listener(string apiKey, IList<String> includeEvents = null)
        {
            this.apiKey = apiKey;
            this.includeEvents = includeEvents;
        }

        public void Start()
        {
            if (this.includeEvents != null && this.includeEvents.Count() > 0)
            {
                foreach(var eventName in this.includeEvents)
                {
                    var pollingUrl = new PollingUrl(this.apiKey, eventName);
                    pollingUrl.start();
                    pollingUrls.Add(pollingUrl);
                }
            }
            else
            {
                var pollingUrl = new PollingUrl(this.apiKey);
                pollingUrl.start();
                pollingUrls.Add(pollingUrl);
            }
        }

        public void End()
        {
            throw new NotImplementedException();
        }

        private class PollingUrl
        {
            private string apiKey;
            private string eventName;
            private UriBuilder requestUri;
            string token = null;

            public PollingUrl(string apiKey,  string eventName = null)
            {
                this.apiKey = apiKey;
                this.eventName = eventName;
                var query = "?";
                if (this.eventName != null)
                {
                    query += "filterBy=eventName&filterValue=" + Uri.EscapeUriString(eventName);
                }
                this.requestUri = new UriBuilder("https://api.hoi.io/events");
                requestUri.Query = query;
            }
          
            internal void start()
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.hoi.io/events?");
            }
        }
    }
}

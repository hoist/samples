using Hoist.SDK.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoist.SDK
{
    public static class Events
    {
        private static Raiser raiser = new Raiser();
        public class HoistEvent
        {
            public string id;
            public string Payload;
            public string eventName;
        }
        
        public static Listener Listen(IList<string> includeEvents = null)
        {
            return new Listener(Configuration.ApiKey, includeEvents);
        }
        public static void Raise(string eventName, object payload)
        {
            raiser.RaiseEvent(eventName, payload);
        }
    }
}

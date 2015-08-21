using Hoist.SDK.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace InvoiceAlerter
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Susbscribing to Hoist Events");
            var subscription = new Hoist.SDK.Events.Listener(@default.Default.HoistAPIKey, includeEvents: new[] { @default.Default.XeroConnectorKey + ":new:invoice" });
            subscription.NewEvent += (Object sender, HoistEvent hoistEvent) => {
                Console.WriteLine(hoistEvent.ToString());
            };
            subscription.Start();
            Console.WriteLine("Listening for events, press any key to end.");
            Console.ReadLine();
            subscription.End();

        }
    }
}

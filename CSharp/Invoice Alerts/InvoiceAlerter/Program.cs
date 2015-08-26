using Hoist.SDK;
using System;
namespace InvoiceAlerter
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Susbscribing to Hoist Events");
            Configuration.ApiKey = @default.Default.HoistAPIKey;
            var subscription = Events.Listen(includeEvents: new[] { @default.Default.XeroConnectorKey + ":new:invoice" });
            subscription.NewEvent += (object sender, Events.HoistEvent hoistEvent) => {
                //raise a new event in hoist to post the event to slack
                Events.Raise("post:to:slack", hoistEvent.Payload);
            };
            subscription.Start();
            Console.WriteLine("Listening for events, press any key to end.");
            Console.ReadLine();
            subscription.End();

        }
    }
}

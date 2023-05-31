using NServiceBus;

namespace Notify
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Notify.Endpoint";
            var endpointConfiguration = new EndpointConfiguration("Notify.Endpoint");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport(new LearningTransport());

            var endpointInstance = await Endpoint.Start(endpointConfiguration);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop();
        }
    }
}
using Topshelf;

namespace Leagues.ReadStore.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ReadStoreService>(s =>
                {
                    s.ConstructUsing(name => new ReadStoreService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Keeps the leagues readstore up-to-date");
                x.SetDisplayName("Leagues ReadStore Processor");
                x.SetServiceName("Leagues.ReadStore.Processor");
            });
        }
    }
}

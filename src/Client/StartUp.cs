using Microsoft.Extensions.DependencyInjection;

namespace Eol.Cpl.Client
{
    /// <summary>
    /// Start up the application by configuring services.
    /// </summary>
    public sealed class Startup
    {
        /// <summary>
        /// Dependency injection for the application is handled here.
        /// </summary>
        /// <param name="serviceCollection"></param>
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IPlayerScoreHelper, PlayerScoreHelper>();
            serviceCollection.AddTransient<IMatchService, MatchService>();
        }
    }
}

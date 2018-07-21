using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Focac_BookService.Startup))]

namespace Focac_BookService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}
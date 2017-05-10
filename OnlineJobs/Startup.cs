using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineJobs.Startup))]
namespace OnlineJobs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

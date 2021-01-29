using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IntraPortal.Startup))]
namespace IntraPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

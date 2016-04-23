using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_30sep.Startup))]
namespace _30sep
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CriptoASPNET.Startup))]
namespace CriptoASPNET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTH.Startup))]
namespace WebTH
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YT1.Startup))]
namespace YT1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

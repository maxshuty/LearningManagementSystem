using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BootcampLMS.UI.Startup))]
namespace BootcampLMS.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LandEconomicsAlumni.Web.Startup))]
namespace LandEconomicsAlumni.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

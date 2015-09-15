using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HSNUAlumni.Web.Startup))]
namespace HSNUAlumni.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

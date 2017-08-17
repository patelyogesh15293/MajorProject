using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MajorProject.Startup))]
namespace MajorProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

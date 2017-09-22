using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Survey_Prototype.Startup))]
namespace Survey_Prototype
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MC_User.Startup))]
namespace MC_User
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

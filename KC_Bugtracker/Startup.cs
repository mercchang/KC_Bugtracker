using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KC_Bugtracker.Startup))]
namespace KC_Bugtracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

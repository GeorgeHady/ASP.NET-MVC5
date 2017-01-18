using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Home_Budget_Manage.Startup))]
namespace Home_Budget_Manage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(myTaiwanTest.Startup))]
namespace myTaiwanTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QTStore.Startup))]
namespace QTStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

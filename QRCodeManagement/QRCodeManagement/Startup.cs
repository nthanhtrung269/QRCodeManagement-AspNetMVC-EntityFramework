using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QRCodeManagement.Startup))]
namespace QRCodeManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

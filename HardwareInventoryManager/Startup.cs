using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HardwareInventoryManager.Startup))]
namespace HardwareInventoryManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

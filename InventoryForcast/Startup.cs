using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InventoryForcast.Startup))]
namespace InventoryForcast
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

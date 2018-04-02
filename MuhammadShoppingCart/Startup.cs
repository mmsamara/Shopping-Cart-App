using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MuhammadShoppingCart.Startup))]
namespace MuhammadShoppingCart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

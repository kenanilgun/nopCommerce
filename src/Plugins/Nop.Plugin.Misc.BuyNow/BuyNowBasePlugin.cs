using Nop.Core.Plugins;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.BuyNow
{
    public class BuyNowBasePlugin : BasePlugin
    {
        public BuyNowBasePlugin()
        {
        }

        public override void Install()
        {
            this.AddOrUpdatePluginLocaleResource("Plugins.BuyNow.Fields.BuyNow", "Buy Now");
            base.Install();
        }

        public override void Uninstall()
        {

            this.DeletePluginLocaleResource("Plugins.BuyNow.Fields.BuyNow");
            base.Uninstall();   
        }
    }
}

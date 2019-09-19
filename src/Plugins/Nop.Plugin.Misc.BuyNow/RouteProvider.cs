using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Payments.PayPalStandard
{
    public partial class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapLocalizedRoute("BuyNow-AddProductToCart-Catalog",
                "Plugins/BuyNow/addproducttocart/catalog/{productId:min(0)}/{shoppingCartTypeId:min(0)}/{quantity:min(0)}",
                 new
                 {
                     controller = "BuyNow",
                     action = "AddProductToCart_Catalog"
                 });

            //add product to cart (with attributes and options). used on the product details pages.
            routeBuilder.MapLocalizedRoute("BuyNow-AddProductToCart-Details", "Plugins/BuyNow/addproducttocart/details/{productId:min(0)}/{shoppingCartTypeId:min(0)}",
                new { controller = "BuyNow", action = "AddProductToCart_Details" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return -1; }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Shipping.Date;
using Nop.Services.Tax;
using Nop.Web.Controllers;
using Nop.Web.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Security.Captcha;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.BuyNow.Controllers
{
    public class BuyNowController : BasePluginController
    {
        #region Fields
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ITaxService _taxService;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ICheckoutAttributeParser _checkoutAttributeParser;
        private readonly IDiscountService _discountService;
        private readonly ICustomerService _customerService;
        private readonly IGiftCardService _giftCardService;
        private readonly IDateRangeService _dateRangeService;
        private readonly ICheckoutAttributeService _checkoutAttributeService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IPermissionService _permissionService;
        private readonly IDownloadService _downloadService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWebHelper _webHelper;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IGenericAttributeService _genericAttributeService;

        private readonly MediaSettings _mediaSettings;
        private readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly OrderSettings _orderSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly CustomerSettings _customerSettings;

        private readonly ShoppingCartController _shoppingCartController;
        private readonly IActionContextAccessor _actionContextAccessor;

        #endregion

        #region Ctor

        public BuyNowController(IShoppingCartModelFactory shoppingCartModelFactory,
            IProductService productService,
            IStoreContext storeContext,
            IWorkContext workContext,
            IShoppingCartService shoppingCartService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            IProductAttributeService productAttributeService,
            IProductAttributeParser productAttributeParser,
            ITaxService taxService, ICurrencyService currencyService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            ICheckoutAttributeParser checkoutAttributeParser,
            IDiscountService discountService,
            ICustomerService customerService,
            IGiftCardService giftCardService,
            IDateRangeService dateRangeService,
            ICheckoutAttributeService checkoutAttributeService,
            IWorkflowMessageService workflowMessageService,
            IPermissionService permissionService,
            IDownloadService downloadService,
            IStaticCacheManager cacheManager,
            IWebHelper webHelper,
            ICustomerActivityService customerActivityService,
            IGenericAttributeService genericAttributeService,
            MediaSettings mediaSettings,
            ShoppingCartSettings shoppingCartSettings,
            OrderSettings orderSettings,
            CaptchaSettings captchaSettings,
            CustomerSettings customerSettings,
            IActionContextAccessor actionContextAccessor)
        {
            _shoppingCartModelFactory = shoppingCartModelFactory;
            _productService = productService;
            _workContext = workContext;
            _storeContext = storeContext;
            _shoppingCartService = shoppingCartService;
            _pictureService = pictureService;
            _localizationService = localizationService;
            _productAttributeService = productAttributeService;
            _productAttributeParser = productAttributeParser;
            _taxService = taxService;
            _currencyService = currencyService;
            _priceCalculationService = priceCalculationService;
            _priceFormatter = priceFormatter;
            _checkoutAttributeParser = checkoutAttributeParser;
            _discountService = discountService;
            _customerService = customerService;
            _giftCardService = giftCardService;
            _dateRangeService = dateRangeService;
            _checkoutAttributeService = checkoutAttributeService;
            _workflowMessageService = workflowMessageService;
            _permissionService = permissionService;
            _downloadService = downloadService;
            _cacheManager = cacheManager;
            _webHelper = webHelper;
            _customerActivityService = customerActivityService;
            _genericAttributeService = genericAttributeService;

            _mediaSettings = mediaSettings;
            _shoppingCartSettings = shoppingCartSettings;
            _orderSettings = orderSettings;
            _captchaSettings = captchaSettings;
            _customerSettings = customerSettings;
            _actionContextAccessor = actionContextAccessor;

            _shoppingCartController = new ShoppingCartController(_shoppingCartModelFactory, _productService, _storeContext, _workContext, _shoppingCartService,
                _pictureService, _localizationService, _productAttributeService, _productAttributeParser, _taxService, _currencyService, _priceCalculationService,
                _priceFormatter, _checkoutAttributeParser, _discountService, _customerService, _giftCardService, _dateRangeService, _checkoutAttributeService, _workflowMessageService,
                _permissionService, _downloadService, _cacheManager, _webHelper, _customerActivityService, _genericAttributeService, _mediaSettings, _shoppingCartSettings,
                _orderSettings, _captchaSettings, _customerSettings);
        }

        #endregion

        #region Methods

        [HttpPost]
        public IActionResult AddProductToCart_Catalog(int productId,
            int shoppingCartTypeId,
            int quantity,
            bool forceredirection = false)
        {
            if (_workContext.CurrentCustomer.IsRegistered())
            {
                _shoppingCartController.ControllerContext = ControllerContext;
                var controllerResult = _shoppingCartController.AddProductToCart_Catalog(productId, shoppingCartTypeId, quantity, forceredirection);

                return controllerResult;
            }
            else
            {
                return Json(new
                {
                    redirect = Url.RouteUrl("login")
                });
            }   
        }

        [HttpPost]
        public IActionResult AddProductToCart_Details(int productId, int shoppingCartTypeId, IFormCollection form)
        {
            if (_workContext.CurrentCustomer.IsRegistered())
            {
                _shoppingCartController.ControllerContext = ControllerContext;
                var controllerResult = _shoppingCartController.AddProductToCart_Details(productId, shoppingCartTypeId, form);

                return controllerResult;
            }
            else
            {
                return Json(new
                {
                    redirect = Url.RouteUrl("login")
                });
            }
        }


        #endregion
    }
}

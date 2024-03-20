using Microsoft.Playwright;

namespace PlaywrightTests.PageObjects{
    public class BasePage
    {
        protected readonly IPage Page;
        public BasePage(IPage page)
        {
            Page = page;
        }
        public async Task NavigateAsync(string url)
        {
            await Page.GotoAsync(url);
        }
    }
    public class LoginPage : BasePage
    {
        private string UsernameSelector => "#user-name";
        private string PasswordSelector => "#password";
        private string LoginButtonSelector => "#login-button";
        public LoginPage(IPage page) : base(page) {}
        public async Task LoginAsync(string username, string password)
        {
            await Page.FillAsync(UsernameSelector, username);
            await Page.FillAsync(PasswordSelector, password);
            await Page.ClickAsync(LoginButtonSelector);
        }
        public async Task<bool> IsLoginPageDisplayedAsync()
        {
            return await Page.IsVisibleAsync(UsernameSelector) && await Page.IsVisibleAsync(LoginButtonSelector);
        }
    }
    public class ProductsPage : BasePage
    {
        private string ProductTitleSelector => ".inventory_item_name";
        private string ProductListSelector => ".inventory_list";
        private string PageTitleSelector => ".title";
        public ProductsPage(IPage page) : base(page) {}
        public async Task ClickProductByNameAsync(string name)
        {
            await Page.ClickAsync($"{ProductTitleSelector}:has-text('{name}')");
        }
        public async Task<bool> IsProductPageDisplayedAsync()
        {
            return await Page.IsVisibleAsync(ProductListSelector);
        }
        public async Task<string> GetPageTitleAsync()
        {
            return await Page.InnerTextAsync(PageTitleSelector);
        }
    }
    public class TShirtDetailsPage : BasePage
    {
        private string AddToCartButtonSelector => "#add-to-cart-sauce-labs-bolt-t-shirt";
        private string ProductTitleSelector => ".inventory_details_name";
        private string CartItemCountSelector => ".shopping_cart_badge";
        public TShirtDetailsPage(IPage page) : base(page) {}

        public async Task<string> GetProductTitleAsync()
        {
            return await Page.InnerTextAsync(ProductTitleSelector);
        }
        public async Task<bool> IsAddToCartButtonDisplayedAsync()
        {
            return await Page.IsVisibleAsync(AddToCartButtonSelector);
        }
        public async Task ClickAddToCartButtonAsync()
        {
            await Page.ClickAsync(AddToCartButtonSelector);
        }
        public async Task<string> GetCartItemCountAsync()
        {
            return await Page.InnerTextAsync(CartItemCountSelector);
        }
    }
    public class CartPage : BasePage
    {
        private string CartItemSelectorRoot => ".cart_item";
        private string CartTitleSelector => ".title";
        private string ProductNameSelector => $"{CartItemSelectorRoot} .inventory_item_name";
        private string ProductPriceSelector => $"{CartItemSelectorRoot} .inventory_item_price";
        private string ProductQuantitySelector => $"{CartItemSelectorRoot} .cart_quantity";
        private string CheckoutButtonSelector => "#checkout";
        public CartPage(IPage page) : base(page) {}
           public async Task<bool> IsCartPageDisplayedAsync()
        {
            return await Page.IsVisibleAsync(CartTitleSelector);
        }
         public async Task<string> GetProductNameAsync()
        {
            return await Page.InnerTextAsync(ProductNameSelector);
        }
        public async Task<string> GetProductPriceAsync()
        {
            return await Page.InnerTextAsync(ProductPriceSelector);
        }
        public async Task<string> GetProductQuantityAsync()
        {
            return await Page.InnerTextAsync(ProductQuantitySelector);
        }
        public async Task<bool> IsProductDetailsCorrectAsync(string expectedName, string expectedPrice, string expectedQuantity)
        {
            var name = await GetProductNameAsync();
            var price = await GetProductPriceAsync();
            var quantity = await GetProductQuantityAsync();

            return name == expectedName && price == expectedPrice && quantity == expectedQuantity;
        }
         public async Task ClickCheckoutButtonAsync()
        {
            await Page.ClickAsync(CheckoutButtonSelector);
        }
    }
    public class CheckoutInformationPage : BasePage
    {
        private string CheckoutInfoFormSelector => "#checkout_info_container";
        private string PageTitleSelector => ".title";
        private string FirstNameSelector => "#first-name";
        private string LastNameSelector => "#last-name";
        private string PostalCodeSelector => "#postal-code";
        private string ContinueButtonSelector => "#continue";
        public CheckoutInformationPage(IPage page) : base(page) {}
        public async Task<bool> IsCheckoutInformationPageDisplayedAsync()
        {
            return await Page.IsVisibleAsync(CheckoutInfoFormSelector);
        }
        public async Task<string> GetPageTitleAsync()
        {
            return await Page.InnerTextAsync(PageTitleSelector);
        }
          public async Task FillOutCheckoutInformationAsync(string firstName, string lastName, string postalCode)
        {
            await Page.FillAsync(FirstNameSelector, firstName);
            await Page.FillAsync(LastNameSelector, lastName);
            await Page.FillAsync(PostalCodeSelector, postalCode);
        }
        public async Task ClickContinueButtonAsync()
        {
            await Page.ClickAsync(ContinueButtonSelector);
        }
    }
    public class OrderSummaryPage : BasePage
    {
        private string PageTitleSelector => ".title";
        private string PriceSelector = ".inventory_item_price"; 
        private string ItemNameSelector => ".inventory_item_name";
        private string TotalAmountSelector => ".summary_total_label";
        private string FinishButtonSelector => "#finish";
        public OrderSummaryPage(IPage page) : base(page) {}
        public async Task<bool> IsOrderSummaryPageDisplayedAsync()
        {
            return await Page.IsVisibleAsync(PageTitleSelector);
        }
        public async Task<string> GetItemNameAsync()
        {
            return await Page.InnerTextAsync(ItemNameSelector);
        }
        public async Task<string> GetTotalAmountAsync()
        {
            return await Page.InnerTextAsync(TotalAmountSelector);
        }
        public async Task<bool> IsTShirtPriceCorrectAsync(string expectedPrice)
        {
            string itemPrice = await Page.InnerTextAsync(PriceSelector);

            return itemPrice == expectedPrice;
        }
        public async Task<bool> AreDetailsCorrectAsync(string expectedName, string expectedTotal)
        {
            string itemName = await GetItemNameAsync();
            string totalAmount = await GetTotalAmountAsync();
            bool isNameCorrect = itemName == expectedName;
            bool isTotalCorrect = totalAmount.Contains(expectedTotal);
            return isNameCorrect && isTotalCorrect;
        }
          public async Task ClickFinishButtonAsync()
        {
            await Page.ClickAsync(FinishButtonSelector);
        }

    }
    public class OrderConfirmationPage : BasePage
    {
        private string ConfirmationMessageSelector => ".complete-header";
        private string HamburgerMenuSelector => "#react-burger-menu-btn";
        private string LogoutButtonSelector => "#logout_sidebar_link";
        public OrderConfirmationPage(IPage page) : base(page) {}
        public async Task<bool> IsConfirmationPageDisplayedAsync()
        {
            return await Page.IsVisibleAsync(ConfirmationMessageSelector);
        }
        public async Task<string> GetConfirmationMessageAsync()
        {
            return await Page.InnerTextAsync(ConfirmationMessageSelector);
        }
           public async Task ClickHamburgerMenu()
        {
            await Page.ClickAsync(HamburgerMenuSelector);
        }
            public async Task ClickLogoutButtonAsync()
        {
            await Page.ClickAsync(LogoutButtonSelector);
        }
    }
}
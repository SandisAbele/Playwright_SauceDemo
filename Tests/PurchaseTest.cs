using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using PlaywrightTests.PageObjects;

public class PurchaseTest : PageTest
{   
    // $env:PWDEBUG="1" --- For quick debugging, set the PWDEBUG environment variable to "1" to launch the browser in headful mode.
    // $env:PWDEBUG="0" --- Headless (the default).
    [Test] 
    public async Task BuyTShirtTest()
    {   
            // STEP 1 - Navigate to the Sauce Labs Sample Application (https://www.saucedemo.com/) in Incognito mode.
            // STEP 2 - Enter valid credentials to log in.
        var loginPage = new LoginPage(Page);
        await loginPage.NavigateAsync("https://www.saucedemo.com/");
        await loginPage.LoginAsync("standard_user", "secret_sauce");

            // STEP 3 - Verify that the login is successful and the user is redirected to the products page.
        var productsPage = new ProductsPage(Page);
        var isProductsPageDisplayed = await productsPage.IsProductPageDisplayedAsync();
        Assert.IsTrue(isProductsPageDisplayed, "The products page was not displayed after login.");
        var pageTitle = await productsPage.GetPageTitleAsync();
        Assert.AreEqual("Products", pageTitle, "The page title is not as expected.");

            // STEP 4 - Select a T-shirt by clicking on its image or name
            // STEP 5 - Verify that the T-shirt details page is displayed.
        await productsPage.ClickProductByNameAsync("Sauce Labs Bolt T-Shirt");
        var tShirtDetailsPage = new TShirtDetailsPage(Page);
        var isAddToCartButtonDisplayed = await tShirtDetailsPage.IsAddToCartButtonDisplayedAsync();
        Assert.IsTrue(isAddToCartButtonDisplayed, "The 'Add to Cart' button was not displayed, indicating the T-shirt details page may not be displayed correctly.");
        var productTitle = await tShirtDetailsPage.GetProductTitleAsync();
        Assert.AreEqual("Sauce Labs Bolt T-Shirt", productTitle, "The product title is not as expected.");

            // STEP 6 - Click the "Add to Cart" button.
        await tShirtDetailsPage.ClickAddToCartButtonAsync();

            // STEP 7 - Verify that the T-shirt is added to the cart successfully.
        string itemCount = await tShirtDetailsPage.GetCartItemCountAsync();
        Assert.AreEqual("1", itemCount, "The cart does not indicate that 1 item has been added.");

            // STEP 8 - Navigate to the cart by clicking the cart icon or accessing the cart page directly.
            // STEP 9 - Verify that the cart page is displayed.
        var cartPage = new CartPage(Page);
        await cartPage.NavigateAsync("https://www.saucedemo.com/cart.html");
        var isCartPageDisplayed = await cartPage.IsCartPageDisplayedAsync();
        Assert.IsTrue(isCartPageDisplayed, "The cart page was not displayed.");

            // STEP 10 - Review the items in the cart and ensure that the T-shirt is listed with the correct details (name, price, quantity, etc.)
        bool isDetailsCorrect = await cartPage.IsProductDetailsCorrectAsync(
        expectedName: "Sauce Labs Bolt T-Shirt",
        expectedPrice: "$15.99",
        expectedQuantity: "1");
        Assert.IsTrue(isDetailsCorrect, "The T-shirt details in the cart do not match the expected values.");

            // STEP 11 - Click the "Checkout" button.
            // STEP 12 - Verify that the checkout information page is displayed.
        await cartPage.ClickCheckoutButtonAsync();
        var checkoutInformationPage = new CheckoutInformationPage(Page);
        bool isCheckoutInformationPageDisplayed = await checkoutInformationPage.IsCheckoutInformationPageDisplayedAsync();
        Assert.IsTrue(isCheckoutInformationPageDisplayed, "The checkout information page was not displayed.");
        string pageTitleCheckout = await checkoutInformationPage.GetPageTitleAsync();
        Assert.AreEqual("Checkout: Your Information", pageTitleCheckout, "The page title is not as expected.");

            // STEP 13 - Enter the required checkout information (e.g., name, shipping address, payment details).
            // STEP 14 - Click the "Continue" button.
        await checkoutInformationPage.FillOutCheckoutInformationAsync("Sandis", "Abele", "LV3001");
        await checkoutInformationPage.ClickContinueButtonAsync();

            // STEP 15 - Verify that the order summary page is displayed, showing the T-shirt details and the total amount.
        var orderSummaryPage = new OrderSummaryPage(Page);
        bool isOrderSummaryPageDisplayed = await orderSummaryPage.IsOrderSummaryPageDisplayedAsync();
        Assert.IsTrue(isOrderSummaryPageDisplayed, "The order summary page was not displayed.");
        bool isTShirtPriceCorrect = await orderSummaryPage.IsTShirtPriceCorrectAsync("$15.99");
        Assert.IsTrue(isTShirtPriceCorrect, "The T-shirt price on the order summary page is not as expected.");
        bool areDetailsCorrect = await orderSummaryPage.AreDetailsCorrectAsync(
        "Sauce Labs Bolt T-Shirt",
        "$17.27"                   
        );
        Assert.IsTrue(areDetailsCorrect, "The order summary details are incorrect.");

            // STEP 16 - Click the "Finish" button.
        await orderSummaryPage.ClickFinishButtonAsync();

            // STEP 17 - Verify that the order confirmation page is displayed, indicating a successful purchase
        var orderConfirmationPage = new OrderConfirmationPage(Page);
        bool isConfirmationPageDisplayed = await orderConfirmationPage.IsConfirmationPageDisplayedAsync();
        Assert.IsTrue(isConfirmationPageDisplayed, "The order confirmation page was not displayed.");
        string confirmationMessage = await orderConfirmationPage.GetConfirmationMessageAsync();
        Assert.IsTrue(confirmationMessage.Contains("Thank you for your order!"), "The confirmation message is not as expected.");

            // STEP 18 - Logout from the application.
        await orderConfirmationPage.ClickHamburgerMenu();
        await orderConfirmationPage.ClickLogoutButtonAsync();

            // STEP 19 - Verify that the user is successfully logged out and redirected to the login page.
        bool isLoginPageDisplayed = await loginPage.IsLoginPageDisplayedAsync();
        Assert.IsTrue(isLoginPageDisplayed, "The user was not successfully logged out and redirected to the login page.");
    }
}

using BasketApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BasketApp.Tests.TestSteps
{
    [Binding]
    public class BasketSteps
    {
        ScenarioContext _context;
        public BasketSteps(ScenarioContext conext)
        {
            _context = conext;
            IBasket basket = new Basket();
            _context["Basket"] = basket;
        }

        [Given(@"I have added the following items to my basket")]
        public void GivenIHaveAddedTheFollowingItemsToMyBasket(Table table)
        {
            List<Product> products = table.CreateSet<Product>().ToList();
            IBasket basket = (IBasket)_context["Basket"];
            basket.AddItemsToBasket(products);
        }

    
        [When(@"I apply the following gift vouchers")]
        public void WhenIApplyTheFollowingGiftVouchers(Table table)
        {
            List<GiftVoucher> vouchers = table.CreateSet<GiftVoucher>().ToList();
            IBasket basket = (IBasket)_context["Basket"];
            foreach (GiftVoucher voucher in vouchers)
            {
                basket.ApplyVoucher(voucher);
            }
        }

        [When(@"I apply the following offer vouchers")]
        public void WhenIApplyTheFollowingOfferVouchers(Table table)
        {
            List<OfferVoucher> vouchers = table.CreateSet<OfferVoucher>().ToList();
            IBasket basket = (IBasket)_context["Basket"];
            foreach (OfferVoucher voucher in vouchers)
            {
                basket.ApplyVoucher(voucher);
            }
        }

        [When(@"I apply the following category offer vouchers")]
        public void WhenIApplyTheFollowingCategoryOfferVouchers(Table table)
        {
            List<CategoryOfferVoucher> vouchers = table.CreateSet<CategoryOfferVoucher>().ToList();
            IBasket basket = (IBasket)_context["Basket"];
            foreach (CategoryOfferVoucher voucher in vouchers)
            {
                basket.ApplyVoucher(voucher);
            }
        }

        [When(@"I then remove the following item from the basket")]
        public void WhenIThenRemoveTheFollowingItemFromTheBasket(Table table)
        {
            List<Product> products = table.CreateSet<Product>().ToList();
            IBasket basket = (IBasket)_context["Basket"];

            foreach (Product product in products)
            {
                basket.RemoveItemFromBasket(product);
            }
        }
        
        [Then(@"The total price should be (.*)")]
        public void ThenTheTotalPriceShouldBe(Decimal expectedTotalPrice)
        {
            IBasket basket = (IBasket)_context["Basket"];
            decimal actualTotalPrice = basket.TotalPrice;
            Assert.AreEqual(expectedTotalPrice, actualTotalPrice);
        }
        
        [Then(@"There should be no error message")]
        public void ThenThereShouldBeNoErrorMessage()
        {
            IBasket basket = (IBasket)_context["Basket"];
            string actualErrorMessage = basket.ErrorMessage;
            Assert.IsTrue(String.IsNullOrEmpty(actualErrorMessage));
        }

        [Then(@"The error message should be ""(.*)""")]
        public void ThenTheErrorMessageShouldBe(string expectedErrorMessage)
        {
            IBasket basket = (IBasket)_context["Basket"];
            string actualErrorMessage = basket.ErrorMessage;
            Assert.IsTrue(expectedErrorMessage == actualErrorMessage);
        }

    }
}

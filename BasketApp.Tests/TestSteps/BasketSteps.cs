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
        
        [Given(@"I have also applied the following gift voucher")]
        public void GivenIHaveAlsoAppliedTheFollowingGiftVoucher(Table table)
        {
            List<GiftVoucher> vouchers = table.CreateSet<GiftVoucher>().ToList();
            IBasket basket = (IBasket)_context["Basket"];
            foreach (GiftVoucher voucher in vouchers)
            {
                basket.ApplyVoucher(voucher);
            }
        }

        [Given(@"I have also applied the following offer voucher")]
        public void GivenIHaveAlsoAppliedTheFollowingOfferVoucher(Table table)
        {
            List<OfferVoucher> vouchers = table.CreateSet<OfferVoucher>().ToList();
            IBasket basket = (IBasket)_context["Basket"];
            foreach (OfferVoucher voucher in vouchers)
            {
                basket.ApplyVoucher(voucher);
            }
        }

        [Given(@"I have also applied the following category offer voucher")]
        public void GivenIHaveAlsoAppliedTheFollowingCategoryOfferVoucher(Table table)
        {
            List<CategoryOfferVoucher> vouchers = table.CreateSet<CategoryOfferVoucher>().ToList();
            IBasket basket = (IBasket)_context["Basket"];
            foreach (CategoryOfferVoucher voucher in vouchers)
            {
                basket.ApplyVoucher(voucher);
            }
        }

        [Given(@"I then remove the following item from the basket")]
        public void GivenIThenRemoveTheFollowingItemFromTheBasket(Table table)
        {
            List<Product> products = table.CreateSet<Product>().ToList();
            IBasket basket = (IBasket)_context["Basket"];

            foreach (Product product in products)
            {
                basket.RemoveItemFromBasket(product);
            }
        }


        [When(@"The basket calculates the total price")]
        public void WhenTheBasketCalculatesTheTotalPrice()
        {
            IBasket basket = (IBasket)_context["Basket"];
            _context["TotalPrice"] = basket.TotalPrice;
            _context["ErrorMessage"] = basket.ErrorMessage;
        }
        
        [Then(@"The total price should be (.*)")]
        public void ThenTheTotalPriceShouldBe(Decimal expectedTotalPrice)
        {
            decimal actualTotalPrice = (decimal)_context["TotalPrice"];
            Assert.AreEqual(expectedTotalPrice, actualTotalPrice);
        }
        
        [Then(@"There should be no error message")]
        public void ThenThereShouldBeNoErrorMessage()
        {
            string actualErrorMessage = (string)_context["ErrorMessage"];
            Assert.IsTrue(String.IsNullOrEmpty(actualErrorMessage));
        }

        [Then(@"The error message should be ""(.*)""")]
        public void ThenTheErrorMessageShouldBe(string expectedErrorMessage)
        {
            string actualErrorMessage = (string)_context["ErrorMessage"];
            Assert.IsTrue(expectedErrorMessage == actualErrorMessage);
        }

    }
}

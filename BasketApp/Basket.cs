using BasketApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketApp
{
    public interface IBasket
    {
        List<Product> BasketItems { get; }
        List<GiftVoucher> AppliedVouchers { get; }
        decimal TotalPrice { get; }
        string ErrorMessage { get; }

        void AddItemsToBasket(List<Product> products);
        void ApplyVoucher(GiftVoucher voucher);
        void RemoveItemFromBasket(Product product);
    }
    public class Basket : IBasket
    {
        List<Product> _basketItems;
        List<GiftVoucher> _appliedVouchers;
        decimal _originalPrice;
        decimal _totalPrice;
        decimal _discountTotal;
        string _errorMessage;

        public Basket()
        {
            _basketItems = new List<Product>();
            _appliedVouchers = new List<GiftVoucher>();
        }

        #region Properties
        public List<Product> BasketItems
        {
            get { return _basketItems; }
        }

        public List<GiftVoucher> AppliedVouchers
        {
            get { return _appliedVouchers; }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        #endregion

        #region Public Methods
        public void AddItemsToBasket(List<Product> products)
        {
            foreach (Product product in products)
            {
                _basketItems.Add(product);
            }
            CalculateOriginalTotalPrice();
        }

        public void ApplyVoucher(GiftVoucher voucher)
        {
            if (IsVoucherValid(voucher))
            {
                _appliedVouchers.Add(voucher);
            }
            CalculateTotalPrice();
        }

        public void RemoveItemFromBasket(Product product)
        {
            Product productToRemove = _basketItems.Where(w => w.Name == product.Name).FirstOrDefault();
            _basketItems.Remove(productToRemove);
            CalculateOriginalTotalPrice();
            ReEvaluateAppliedVouchers();
        }

        #endregion

        #region Private Methods
        bool IsVoucherValid(GiftVoucher voucher)
        {
            bool isVoucherValid = false;

            if (voucher.GetType() == typeof(GiftVoucher))
            {
                isVoucherValid = true;
                _discountTotal = _discountTotal + voucher.Discount;
            }
            else if (!_appliedVouchers.Any(a => a.GetType() == typeof(OfferVoucher) && !_appliedVouchers.Any(z => z.GetType() == typeof(CategoryOfferVoucher))))
            {
                if (voucher.GetType() == typeof(OfferVoucher))
                {
                    OfferVoucher offerVoucher = (OfferVoucher)voucher;
                    isVoucherValid = IsOfferVoucherValid(offerVoucher);
                }
                else if (voucher.GetType() == typeof(CategoryOfferVoucher))
                {
                    CategoryOfferVoucher offerVoucher = (CategoryOfferVoucher)voucher;
                    isVoucherValid = IsCategoryOfferVoucherValid(offerVoucher);
                }
            }
            else
            {
                _errorMessage = "You can only use one offer voucher at a time.";
            }

            return isVoucherValid;
        }

        bool IsOfferVoucherValid(OfferVoucher voucher)
        {
            bool isValid = IsVoucherThresholdMet(voucher);
            _discountTotal = (isValid) ? _discountTotal + voucher.Discount : _discountTotal;
            return isValid;
        }

        bool IsCategoryOfferVoucherValid(CategoryOfferVoucher voucher)
        {
            bool isValid = false;

            if (IsVoucherThresholdMet(voucher))
            {
                List<Product> qualifyingProducts = BasketItems.Where(w => w.Category == voucher.ProductCategory).ToList();

                if (qualifyingProducts.Count > 0)
                {
                    decimal qualifyingProductsTotalPrice = 0;
                    decimal adjustedProductDiscount = 0;

                    foreach (Product product in qualifyingProducts)
                    {
                        qualifyingProductsTotalPrice = qualifyingProductsTotalPrice + product.Price;
                    }
                    if (voucher.Discount > qualifyingProductsTotalPrice)
                    {
                        adjustedProductDiscount = adjustedProductDiscount + qualifyingProductsTotalPrice;
                    }

                    isValid = true;
                    _discountTotal = _discountTotal + adjustedProductDiscount;
                }
                else
                {
                    _errorMessage = $"There are no products in your basket applicable to voucher {voucher.VoucherCode}.";
                }
            }
            return isValid;
        }

        bool IsVoucherThresholdMet(OfferVoucher voucher)
        {
            bool isValid = false;
            const decimal thresholdBoundary = 0.01m;
            decimal adjustedPrice = _originalPrice;

            List<Product> invalidThresholdItems = BasketItems.Where(w => w.Category == "Gift Vouchers").ToList();

            if (invalidThresholdItems.Count > 0)
            {
                decimal totalInvalidCost = 0;
                foreach (Product product in invalidThresholdItems)
                {
                    totalInvalidCost = totalInvalidCost + product.Price;
                }
                adjustedPrice = (adjustedPrice - totalInvalidCost);
            }

            decimal remainingCostToReachThreshold = (voucher.Threshold - adjustedPrice) + thresholdBoundary;
            
            if ((invalidThresholdItems.Count() == 0 && _originalPrice > voucher.Threshold) || remainingCostToReachThreshold <= 0)
            {
                isValid = true;
            }
            else
            {
                _errorMessage = $"You have not reached the spend threshold for the voucher {voucher.VoucherCode}. Spend another £{remainingCostToReachThreshold} to receive the £{voucher.Discount} discount from your basket total.";
            }

            return isValid;
        }

        void CalculateOriginalTotalPrice()
        {
            decimal totalProductPrice = 0;

            foreach (Product product in BasketItems)
            {
                totalProductPrice = totalProductPrice + product.Price;
            }

            _originalPrice = totalProductPrice;
        }

        void CalculateTotalPrice()
        {
            _totalPrice = _originalPrice - _discountTotal;
        }

        void ReEvaluateAppliedVouchers()
        {
            _discountTotal = 0;
            List<GiftVoucher> appliedVouchers = new List<GiftVoucher>();
            foreach (GiftVoucher voucher in AppliedVouchers)
            {
                appliedVouchers.Add(voucher);
            }

            _appliedVouchers = new List<GiftVoucher>();

            foreach (GiftVoucher voucher in appliedVouchers)
            {
                ApplyVoucher(voucher);
            }
        }
        #endregion
    }
}

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
        List<Voucher> AppliedVouchers { get; }
        decimal TotalPrice { get; }
        string ErrorMessage { get; }

        void AddItemsToBasket(List<Product> products);
        void ApplyVoucher(Voucher voucher);
        void RemoveItemFromBasket(Product product);
    }
    public class Basket : IBasket
    {
        List<Product> _basketItems;
        List<Voucher> _appliedVouchers;
        decimal _originalPrice;
        decimal _totalPrice;
        decimal _discountPrice;
        string _errorMessage;

        public Basket()
        {
            _basketItems = new List<Product>();
            _appliedVouchers = new List<Voucher>();
        }

        public List<Product> BasketItems
        {
            get { return _basketItems; }
        }

        public List<Voucher> AppliedVouchers
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

        public void AddItemsToBasket(List<Product> products)
        {
            foreach (Product product in products)
            {
                _basketItems.Add(product);
            }
            CalculateOriginalTotalPrice();
        }

        public void ApplyVoucher(Voucher voucher)
        {
            if (IsVoucherValid(voucher))
            {
                _appliedVouchers.Add(voucher);
            }
            CalculateTotalPrice();
        }

        bool IsVoucherValid(Voucher voucher)
        {
            bool isVoucherValid = false;

            if (voucher.GetType() == typeof(Voucher))
            {
                isVoucherValid = true;
                _discountPrice = _discountPrice + voucher.Discount;
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
            _discountPrice = (isValid) ? _discountPrice + voucher.Discount : _discountPrice;
            return isValid;
        }

        bool IsCategoryOfferVoucherValid(CategoryOfferVoucher voucher)
        {
            bool isValid = false;

            if (IsVoucherThresholdMet(voucher))
            {
                List<Product> qualifyingProducts = BasketItems.Where(w => w.Category == voucher.ProductCategory).ToList();

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
                if (qualifyingProducts.Count() > 0)
                {
                    isValid = true;
                    _discountPrice = _discountPrice + adjustedProductDiscount;
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
            _totalPrice = _originalPrice - _discountPrice;
        }

        public void RemoveItemFromBasket(Product product)
        {
            Product productToRemove = _basketItems.Where(w => w.Name == product.Name).FirstOrDefault();
            _basketItems.Remove(productToRemove);
            CalculateOriginalTotalPrice();
            ReEvaluateAppliedVouchers();
        }

        void ReEvaluateAppliedVouchers()
        {
            _discountPrice = 0;
            List<Voucher> appliedVouchers = new List<Voucher>();
            foreach (Voucher voucher in AppliedVouchers)
            {
                appliedVouchers.Add(voucher);
            }

            _appliedVouchers = new List<Voucher>();

            foreach (Voucher voucher in appliedVouchers)
            {
                ApplyVoucher(voucher);
            }
        }
    }
}

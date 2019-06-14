using System;
using System.Collections.Generic;
using System.Text;

namespace BasketApp.Models
{
    public class GiftVoucher
    {
        string _voucherCode;
        decimal _discount;

        public GiftVoucher(string voucherCode, decimal discount)
        {
            _voucherCode = voucherCode;
            _discount = discount;
        }

        public string VoucherCode
        {
            get { return _voucherCode; }
        }

        public decimal Discount
        {
            get { return _discount; }
        }
    }

    public class OfferVoucher : GiftVoucher
    {
        decimal _threshold;

        public OfferVoucher(string voucherCode, decimal discount, decimal threshold) : base(voucherCode, discount)
        {
            _threshold = threshold;
        }

        public decimal Threshold
        {
            get { return _threshold; }
        }
    }

    public class CategoryOfferVoucher : OfferVoucher
    {
        string _productCategory;

        public CategoryOfferVoucher(string voucherCode, decimal discount, decimal threshold, string productCategory) : base(voucherCode, discount, threshold)
        {
            _productCategory = productCategory; 
        }

        public string ProductCategory
        {
            get { return _productCategory; }
        }
    }
}

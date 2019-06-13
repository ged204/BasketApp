using System;
using System.Collections.Generic;
using System.Text;

namespace BasketApp.Models
{
    public class Product
    {
        string _name;
        decimal _price;
        string _category;

        public Product(string name, decimal price, string category)
        {
            _name = name;
            _price = price;
            _category = category;
        }

        public string Name
        {
            get { return _name; }
        }

        public decimal Price
        {
            get { return _price; }
        }

        public string Category
        {
            get { return _category; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BasketApp.Models
{
    public class ProductCategory
    {
        int _id;
        string _name;

        public ProductCategory(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}

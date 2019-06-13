using BasketApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow.Assist;

namespace BasketApp.Tests.Helpers
{
    public class ProductCategoryValueRetriever : IValueRetriever
    {
        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            bool success =  (propertyType == typeof(ProductCategory));
            return success;
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            int id;
            string name = keyValuePair.Value;

            bool successparse = int.TryParse(keyValuePair.Key, out id);

            if (successparse)
            {
                ProductCategory category = new ProductCategory(id, name);
                return category;
            }
            else
            {
                throw new Exception($"Unable to parse the product category id {keyValuePair.Key}");
            }
        }
    }
}

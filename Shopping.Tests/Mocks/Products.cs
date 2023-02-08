using Shopping.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Tests.Mocks
{
    public static class Products
    {
        public static Product EmptyProduct()
        {
            return new Product();
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using LINQtoXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoXML.Tests
{
    [TestClass()]
    public class CustomerSorterTests
    {
        [TestMethod()]
        public void SortCustomersByCountryTest()
        {
            var a = new CustomerSorter();
            a.SortCustomersByCountry();
            
        }
    }
}
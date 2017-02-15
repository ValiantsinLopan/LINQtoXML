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

        [TestMethod()]
        public void CustomersWithTotalSumTest()
        {
            var a = new CustomerSorter();
            a.CustomersWithTotalSum(4300);
        }

        [TestMethod()]
        public void CustomersWithOrderSumMoreThenTest()
        {
            var a = new CustomerSorter();
            a.CustomersWithOrderSumMoreThen(880);
        }

        [TestMethod()]
        public void NoDigitInPostCodeNoRegionNoPhoneCodeTest()
        {
            var a = new CustomerSorter();
            a.NoDigitInPostCodeNoRegionNoPhoneCode();
        }

        [TestMethod()]
        public void FirstOrderDateTest()
        {
            var a = new CustomerSorter();
            a.FirstOrderDate();
        }

        [TestMethod()]
        public void AverageProfitAndIntensityInCityTest()
        {
            var a = new CustomerSorter();
            a.AverageProfitAndIntensityInCity();
        }

        [TestMethod()]
        public void OrderByYearMounthTurnoverNameTest()
        {
            var a = new CustomerSorter();
            a.OrderByYearMounthTurnoverName();
        }

        [TestMethod()]
        public void StatisticsTest()
        {
            var a = new CustomerSorter();
            a.Statistics();
        }
    }
}
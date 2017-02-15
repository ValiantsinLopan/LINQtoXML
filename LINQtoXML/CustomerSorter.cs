using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
namespace LINQtoXML
{
    public class CustomerSorter
    {
        public static XDocument xdoc = XDocument.Load(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\RD. HW - AT Lab#. 05 - Customers.xml");

        public void SortCustomersByCountry()
        {
            var groups =
                from customer in xdoc.Descendants("customer")
                group customer by new { country = customer.Element("country").Value } into g
                select g;

            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\2.txt"))
            {
                foreach (var group in groups)
                {
                    file.WriteLine("Country: {0}", group.Key.country);
                    foreach (var customer in group)
                    {
                        file.WriteLine("\tCustomer Name: {0}", customer.Element("name").Value);
                    }
                    file.WriteLine();
                }
            }
        }

        public void CustomersWithTotalSum(double X)
        {
            var customers =
                from el in xdoc.Descendants("customer")
                where el.Descendants("total").Sum(e => double.Parse(e.Value)) > X
                select el;
            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\1.txt"))
            {
                file.WriteLine($"Customers with total sum of orders more then {X}\n");
                foreach (var customer in customers)
                {
                    file.WriteLine(customer.Element("name").Value);
                }
            }
        }

        public void CustomersWithOrderSumMoreThen(double X)
        {
            var customers = xdoc.Element("customers")
                .Elements("customer")
                .Where(el => el.Element("orders").Elements("order").Any(element => double.Parse(element.Element("total").Value) > X));
            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\3.txt"))
            {
                file.WriteLine($"Customers with 'total' value more then {X}\n");
                foreach (var customer in customers)
                {
                    file.WriteLine(customer.Element("name").Value);
                }
            }
        }

        public void NoDigitInPostCodeNoRegionNoPhoneCode()
        {
            var customers =
                from c in xdoc.Element("customers").Elements("customer")
                where c.Elements("postalcode").Any(e => e.Value.Any(x => char.IsLetter(x))) ||
                      c.Elements("phone").Any(e => e.Value.Contains("(")) ||
                      c.Elements("region").Any() == false
                select c;
            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\6.txt"))
            {
                foreach (var customer in customers)
                {
                    file.WriteLine("Customer: {0},\tPostalcode: {1},\tPhone: {2}",
                        customer.Element("name").Value,
                        customer.Element("postalcode").Value,
                        customer.Element("phone").Value);
                }

            }

        }

        public void FirstOrderDate()
        {
            var customers =
                from c in xdoc.Element("customers").Elements("customer")
                where (c.Descendants("order").Any() == true)
                select c;
            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\4.txt"))
            {
                foreach (var customer in customers)
                {
                    var date = DateTime.Parse(customer.Descendants("orderdate").First().Value);
                    file.WriteLine($"{customer.Element("name").Value} is customer from {date.Month} {date.Year}");
                    
                }
            }
        }

        public void AverageProfitAndIntensityInCity()
        {
            var cities =
                from customer in xdoc.Descendants("customer")
                group customer by new { city = customer.Element("city").Value } into c
                select c;
            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\7.txt"))
            {
                foreach (var custInCity in cities)
                {
                    var average = custInCity.Descendants("total").Average(sum => double.Parse(sum.Value));
                    var intensity = (double)custInCity.Descendants("order").Count() / (double)custInCity.Count();

                    file.WriteLine($"In {custInCity.Key.city} average profit is  {average}, intensity of orders: {intensity}");
                    file.WriteLine("\tCustomers:");
                    foreach (var customer in custInCity)
                    {
                        file.WriteLine($"\t{customer.Element("name").Value}");
                    }
                    file.WriteLine();
                }
            }

        }
















    }
}

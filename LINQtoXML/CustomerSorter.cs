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
                    file.WriteLine(customer.Element("name")?.Value);
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
                where c.Elements("postalcode").Any() != c.Elements("postalcode").Any(e => e.Value.All(x => char.IsDigit(x))) ||
                      c.Elements("phone").Any() != c.Elements("phone").Any(e=>e.Value.Contains("(")) ||
                      !c.Elements("region").Any()
                select c;
            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\6.txt"))
            {
                foreach (var customer in customers)
                {
                    file.WriteLine($"{customer.Element("name")?.Value,-40}\tPostalcode: {customer.Element("postalcode")?.Value,5}\tPhone: {customer.Element("phone")?.Value,5}");
                }

            }

        }

        public void FirstOrderDate()
        {
            var customers =
                from c in xdoc.Element("customers").Elements("customer")
                where c.Descendants("order").Any() == true
                select c;
            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\4.txt"))
            {
                foreach (var customer in customers)
                {
                    var date = DateTime.Parse(customer.Descendants("orderdate").First().Value);
                    file.WriteLine(string.Format($"{ customer.Element("name").Value,-40}\t{date.Month,5} { date.Year,5}"));
                    
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
                foreach (var customer in cities)
                {
                    var average = customer.Descendants("total").Average(sum => double.Parse(sum.Value));
                    var intensity = (double)customer.Descendants("order").Count() / (double)customer.Count();

                    file.WriteLine($"In {customer.Key.city} average profit is  {average}, intensity of orders: {intensity}");
                    file.WriteLine("\tCustomers:");
                    foreach (var el in customer)
                    {
                        file.WriteLine($"\t{el.Element("name").Value}");
                    }
                    file.WriteLine();
                }
            }

        }

        public void OrderByYearMounthTurnoverName()
        {
            
            var customersByYear =
                 from c in xdoc.Descendants("customer")
                 where c.Descendants("order").Any() == true
                 orderby DateTime.Parse(c.Descendants("orderdate").First().Value).Year
                 select c;

            var customersByMonth =
                 from c in xdoc.Descendants("customer")
                 where c.Descendants("order").Any() == true
                 orderby DateTime.Parse(c.Descendants("orderdate").First().Value).Month
                 select c;
            var customersByTurnover = 
                from c in xdoc.Descendants("customer")
                where c.Descendants("order").Any() == true
                orderby c.Descendants("total").Sum(e => double.Parse(e.Value)) descending
                select c;
            var customersByName =
                from c in xdoc.Descendants("customer")
                orderby c.Element("name").Value
                select c;
            
            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\5.txt"))
            {
                file.WriteLine("\tCustomers sorted by year of first order");
                file.WriteLine();
                foreach (var customer in customersByYear)
                {
                    file.WriteLine(string.Format($"{DateTime.Parse(customer.Descendants("orderdate").First().Value).Year,-5}\t{customer.Element("name").Value,5} "));

                }
                file.WriteLine();
                file.WriteLine("\tCustomers sorted by mounth of first order");
                file.WriteLine();
                foreach (var customer in customersByMonth)
                {
                    var month = DateTime.Parse(customer.Descendants("orderdate").First().Value).Month;
                    var year = DateTime.Parse(customer.Descendants("orderdate").First().Value).Year;
                    file.WriteLine(string.Format($"{month,-5}{year,-5}\t{customer.Element("name").Value,5} "));
                }
                file.WriteLine();
                file.WriteLine("\tCustomers sorted by turnover");
                file.WriteLine();
                foreach (var customer in customersByTurnover)
                {
                    file.WriteLine($"{customer.Descendants("total").Sum(e => double.Parse(e.Value)),-10}\t{customer.Element("name").Value}");
                }
                file.WriteLine();
                file.WriteLine("\tCustomers sorted by name");
                file.WriteLine();
                foreach (var customer in customersByName)
                {
                    file.WriteLine($"{customer.Element("name").Value,-5}");
                }
            }
        }

        public void Statistics()
        {
            var byMonth =
                 from orders in xdoc.Descendants("order")
                 group orders by new { month = DateTime.Parse(orders.Element("orderdate").Value).Month } into c
                 orderby c.Key.month
                 select c;

            var byYear =
                 from orders in xdoc.Descendants("order")
                 group orders by new { year = DateTime.Parse(orders.Element("orderdate").Value).Year } into c
                 orderby c.Key.year
                 select c;
            /*
                        var byYearMonth =
                             from orders in xdoc.Descendants("order")
                             group orders by new { month = DateTime.Parse(orders.Element("orderdate").Value).Month} into months
                             group months  by new { year = DateTime.Parse(months.Element("orderdate").Value).Year } into years
                             select years;

                        foreach (var year in byYearMonth)
                        {
                            Console.WriteLine($"Year: {year.Key.year} {year.Count()}");

                                /*foreach(var month in year)
                            {
                                Console.WriteLine($"Month:  {month.c.Count()}");
                            }
                        }
            */
            using (StreamWriter file = new StreamWriter(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\8.txt"))
            {
                foreach (var month in byMonth)
                {
                    file.WriteLine($"Month: {month.Key.month}");
                    file.WriteLine($"Number of orders: {month.Count()}");
                }
                file.WriteLine();
                foreach (var year in byYear)
                {
                    file.WriteLine($"Year: {year.Key.year}");
                    file.WriteLine($"Number of orders: {year.Count()}");
                }

            }
        }
        
    }
}

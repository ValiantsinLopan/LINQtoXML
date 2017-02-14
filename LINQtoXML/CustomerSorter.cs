using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
namespace LINQtoXML
{
    public class CustomerSorter
    {
        public static XDocument xdoc = XDocument.Load(@"D:\VALIANTSIN\TAT LAB\LINQtoXML\Data\RD. HW - AT Lab#. 05 - Customers.xml");
        
        public void SortCustomersByCountry()
        {
            var groups =
                from customer in xdoc.Descendants("customer")
                group customer by new { country = (string)customer.Element("country") } into g
                select new { g.Key,g};

            foreach (var group in groups)
            {
                Console.WriteLine("Country: {0}",group.Key.country);
                foreach (var customer in group.g)
                {
                    Console.WriteLine("\tCustomer Name: {0}", (string)customer.Element("name"));
                }
                Console.WriteLine();
            }
        }

        public void CustomersWithTotalSum(double X)
        {
            var customers =
                from el in xdoc.Descendants("customer")
                where el.Descendants("total").Sum(e => double.Parse(e.Value)) > X
                select el;

            Console.WriteLine($"Customers with total sum of orders more then {X}");    
            foreach (var customer in customers)
            {
                Console.WriteLine(customer.Element("name").Value);
            }
        }

        public void CustomersWithOrderSumMoreThen(double X)
        {
            var customers = xdoc.Element("customers")
                .Elements("customer")
                .Where(el => el.Element("orders").Elements("order").Any(element => double.Parse(element.Element("total").Value) > X));
                

            Console.WriteLine($"Customers with orders total more then {X}");
            foreach (var customer in customers)
            {
                Console.WriteLine(customer.Element("name").Value);
            }

        }
    }
}

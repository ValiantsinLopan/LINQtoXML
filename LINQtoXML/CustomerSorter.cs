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
        
        public void SortCustomersByCountry()
        {
            XDocument xdoc = XDocument.Load(@"D:\VALIANTSIN\TAT LAB\RD. HW - AT Lab#. 05 - Customers.xml");
            var groups =
                from customer in xdoc.Root.Elements("customer")
                group customer by new { country = (string)customer.Element("country") } into g
                select new { g.Key,g};
            foreach(var group in groups)
            {
                Console.WriteLine("Country: {0}",group.Key.country);
                foreach (var customer in group.g)
                {
                    Console.WriteLine("\tCustomer Name: {0}; Customer ID: {1}", (string)customer.Element("name"), (string)customer.Element("id"));
                }
                Console.WriteLine();
            }
           // Console.ReadKey();    
        }
    }
}

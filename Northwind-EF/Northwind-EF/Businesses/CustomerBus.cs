using Northwind_EF;
using Northwind_EF.VMs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF.Businesses
{
    internal class CustomerBus
    {
        public static void PrintData(Customer element, int customerPosition)
        {
            var vm = new CustomerVM
            {
                CustomerID = element.CustomerID,
                CompanyName = element.CompanyName,
                ContactName = element.ContactName,
                ContactTitle = element.ContactTitle,
                Address = element.Address,
                City = element.City,
                Region = element.Region,
                PostalCode = element.PostalCode,
                Country = element.Country,
                Phone = element.Phone,
                Fax = element.Fax,
                Orders = element.Orders?.Select(o => new OrderVM
                {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    ShippedDate = o.ShippedDate,
                    ShipName = o.ShipName
                }).ToList(),
                CustomerDemographics = element.CustomerDemographics?.Select(cd => new CustomerDemographicVM
                {
                    CustomerTypeID = cd.CustomerTypeID,
                    CustomerDesc = cd.CustomerDesc
                }).ToList()
            };

            string customer = $"\nCustomer ID: {vm.CustomerID}\nCompany Name: {vm.CompanyName}\nContact Name: {vm.ContactName}\nContact Title: {vm.ContactTitle}\nAddress: {vm.Address}\nCity: {vm.City}\nRegion: {vm.Region}\nPostal Code: {vm.PostalCode}\nCountry: {vm.Country}\nPhone: {vm.Phone}\nFax: {vm.Fax}";

            if (vm.Orders != null && vm.Orders.Any())
            {
                string ordersInfo = "\nOrders:";
                foreach (var order in vm.Orders)
                {
                    ordersInfo += $"\n - Order ID: {order.OrderID}, Order Date: {order.OrderDate}, Shipped Date: {order.ShippedDate}, Ship Name: {order.ShipName}";
                }
                customer += ordersInfo;
            }

            if (vm.CustomerDemographics != null && vm.CustomerDemographics.Any())
            {
                string demographicsInfo = "\nCustomer Demographics:";
                foreach (var demographic in vm.CustomerDemographics)
                {
                    demographicsInfo += $"\n - Customer Type ID: {demographic.CustomerTypeID}, Description: {demographic.CustomerDesc}";
                }
                customer += demographicsInfo;
            }

            Console.WriteLine(customer);
        }

        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Customers.Include(c => c.Orders).Include(c => c.CustomerDemographics).ToList();
                int currentCustomerPosition = 0;
                bool flag = true;

                while (flag)
                {
                    Console.Clear();
                    if (!elements.Any())
                    {
                        Console.WriteLine("No entries available.");
                        Console.WriteLine("Press any key to return to the menu.");
                        Console.ReadKey();
                        flag = false;
                        break;
                    }

                    var element = elements.ElementAt(currentCustomerPosition);
                    PrintData(element, currentCustomerPosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentCustomerPosition = (currentCustomerPosition > 0) ? currentCustomerPosition - 1 : currentCustomerPosition;
                            break;
                        case ConsoleKey.S:
                            currentCustomerPosition = (currentCustomerPosition < elements.Count - 1) ? currentCustomerPosition + 1 : currentCustomerPosition;
                            break;
                        case ConsoleKey.X:
                            context.Customers.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentCustomerPosition);
                            currentCustomerPosition = (currentCustomerPosition >= elements.Count) ? elements.Count - 1 : currentCustomerPosition;
                            Console.WriteLine("\nEntry Deleted!\nPress any key to return to menu");
                            Console.ReadKey();
                            break;
                        case ConsoleKey.N:
                            Console.Clear();
                            // Add new entry logic
                            break;
                        case ConsoleKey.U:
                            Console.Clear();
                            // Update entry logic
                            break;
                        case ConsoleKey.Escape:
                            flag = false;
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("\n Invalid Option! Try Again.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            return;
        }
    }
}

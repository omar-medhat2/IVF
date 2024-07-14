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
    internal class OrderBus
    {
        public static void PrintData(Order element, int customerPosition)
        {
            var vm = new OrderVM
            {
                OrderID = element.OrderID,
                CustomerID = element.CustomerID,
                EmployeeID = element.EmployeeID,
                OrderDate = element.OrderDate,
                ShippedDate = element.ShippedDate,
                ShipName = element.ShipName,
                ShipAddress = element.ShipAddress,
                ShipCity = element.ShipCity,
                ShipPostalCode = element.ShipPostalCode,
                ShipCountry = element.ShipCountry,
                Customer = element.Customer != null ? new CustomerVM
                {
                    CustomerID = element.Customer.CustomerID
                } : null,

                Employee = element.Employee != null ? new EmployeeVM
                {
                    EmployeeID = element.Employee.EmployeeID
                } : null,

                Shipper = element.Shipper != null ? new ShipperVM {
                    ShipperID = element.Shipper.ShipperID
                } : null,

            };
            string order = $"\nOrder ID: {vm.OrderID}\nCustomer ID: {vm.CustomerID}\nEmployee ID: {vm.EmployeeID}\nOrder Date: {vm.OrderDate}\nShipped Date: {vm.ShippedDate}\nShip Name: {vm.ShipName}\nShip Address: {vm.ShipAddress}\nShip City: {vm.ShipCity}\nShip Postal Code: {vm.ShipPostalCode}\nShip Country: {vm.ShipCountry}";

            if (vm.Customer != null)
            {
                string customerInfo = $"\nCustomer ID: {vm.Customer.CustomerID}";
                order += customerInfo;
            }

            if (vm.Employee != null)
            {
                string employeeInfo = $"\nEmployee ID: {vm.Employee.EmployeeID}";
                order += employeeInfo;
            }

            if (vm.Shipper != null)
            {
                string shipperInfo = $"\nShipper ID: {vm.Shipper.ShipperID}";
                order += shipperInfo;
            }

            Console.WriteLine(order);
        }
        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.Shipper).ToList();
                int currentOrderPosition = 0;
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

                    var element = elements.ElementAt(currentOrderPosition);
                    PrintData(element, currentOrderPosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentOrderPosition = (currentOrderPosition > 0) ? currentOrderPosition - 1 : currentOrderPosition;
                            break;
                        case ConsoleKey.S:
                            currentOrderPosition = (currentOrderPosition < elements.Count - 1) ? currentOrderPosition + 1 : currentOrderPosition;
                            break;
                        case ConsoleKey.X:
                            context.Orders.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentOrderPosition);
                            currentOrderPosition = (currentOrderPosition >= elements.Count) ? elements.Count - 1 : currentOrderPosition;
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

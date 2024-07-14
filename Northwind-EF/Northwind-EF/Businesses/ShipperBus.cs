using Northwind_EF.VMs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF.Businesses
{
    public class ShipperBus
    {
        public static void PrintData(Shipper element, int orderPosition)
        {
            ShipperVM vm = new ShipperVM
            {
                ShipperID = element.ShipperID,
                CompanyName = element.CompanyName,
                Phone = element.Phone,
                Orders = element.Orders.Select(o => new OrderVM 
                {
                    OrderID = o.OrderID,
                    CustomerID = o.CustomerID,
                    ShipName = o.ShipName,
                    OrderDate = o.OrderDate,
                    ShippedDate = o.ShippedDate
                }).ToList()
            };

            if (vm.Orders.Any())
            {
                var order = vm.Orders.ElementAt(orderPosition);
                string orderInfo = $"\nCustomer ID: {order.CustomerID}\nOrder ID: {order.OrderID}\nShip Name: {order.ShipName}\nOrder Date: {order.OrderDate}\nShipped Date: {order.ShippedDate}";
                Console.WriteLine($"Shipper ID: {vm.ShipperID}\nCompany Name: {vm.CompanyName}\nPhone: {vm.Phone}\nOrder Info: {orderInfo}");
            }
            else
            {
                Console.WriteLine($"Shipper ID: {vm.ShipperID}\nCompany Name: {vm.CompanyName}\nPhone: {vm.Phone}\nNo orders available for this shipper.");
            }
        }

        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Shippers.Include(s => s.Orders).ToList(); // Include Orders to ensure they are loaded
                int currentShipperPosition = 0;
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

                    var element = elements.ElementAt(currentShipperPosition);

                    PrintData(element, currentOrderPosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'A' to move to the previous order.\nPress 'D' to move to the next order.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentShipperPosition = (currentShipperPosition > 0) ? currentShipperPosition - 1 : currentShipperPosition;
                            currentOrderPosition = 0; // Reset order position when changing shippers
                            break;
                        case ConsoleKey.S:
                            currentShipperPosition = (currentShipperPosition < elements.Count - 1) ? currentShipperPosition + 1 : currentShipperPosition;
                            currentOrderPosition = 0; // Reset order position when changing shippers
                            break;
                        case ConsoleKey.A:
                            currentOrderPosition = (currentOrderPosition > 0) ? currentOrderPosition - 1 : currentOrderPosition;
                            break;
                        case ConsoleKey.D:
                            currentOrderPosition = (currentOrderPosition < element.Orders.Count - 1) ? currentOrderPosition + 1 : currentOrderPosition;
                            break;
                        case ConsoleKey.X:
                            context.Shippers.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentShipperPosition);
                            currentShipperPosition = (currentShipperPosition >= elements.Count) ? elements.Count - 1 : currentShipperPosition;
                            currentOrderPosition = 0; // Reset order position when changing shippers
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

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
    internal class OrderDetailBus
    {
        public static void PrintData(Order_Detail element, int orderDetailPosition)
        {
            Order_DetailVM vm = new Order_DetailVM
            {
                OrderID = element.OrderID,
                ProductID = element.ProductID,
                UnitPrice = element.UnitPrice,
                Quantity = element.Quantity,
                Discount = element.Discount,
                Order = element.Order != null ? new OrderVM
                {
                    OrderID = element.Order.OrderID,
                    OrderDate = element.Order.OrderDate,
                    ShippedDate = element.Order.ShippedDate,
                    ShipName = element.Order.ShipName
                } : null,
                Product = element.Product != null ? new ProductVM
                {
                    ProductID = element.Product.ProductID,
                    ProductName = element.Product.ProductName,
                    QuantityPerUnit = element.Product.QuantityPerUnit,
                    UnitPrice = element.Product.UnitPrice
                } : null
            };

            string orderDetail = $"\nOrder ID: {vm.OrderID}\nProduct ID: {vm.ProductID}\nUnit Price: {vm.UnitPrice}\nQuantity: {vm.Quantity}\nDiscount: {vm.Discount}";

            if (vm.Order != null)
            {
                string orderInfo = $"\nOrder Date: {vm.Order.OrderDate}\nShipped Date: {vm.Order.ShippedDate}\nShip Name: {vm.Order.ShipName}";
                orderDetail += orderInfo;
            }

            if (vm.Product != null)
            {
                string productInfo = $"\nProduct Name: {vm.Product.ProductName}\nQuantity Per Unit: {vm.Product.QuantityPerUnit}\nProduct Unit Price: {vm.Product.UnitPrice}";
                orderDetail += productInfo;
            }

            Console.WriteLine(orderDetail);
        }

        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Order_Details.Include(od => od.Order).Include(od => od.Product).ToList();
                int currentOrderDetailPosition = 0;
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

                    var element = elements.ElementAt(currentOrderDetailPosition);
                    PrintData(element, currentOrderDetailPosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentOrderDetailPosition = (currentOrderDetailPosition > 0) ? currentOrderDetailPosition - 1 : currentOrderDetailPosition;
                            break;
                        case ConsoleKey.S:
                            currentOrderDetailPosition = (currentOrderDetailPosition < elements.Count - 1) ? currentOrderDetailPosition + 1 : currentOrderDetailPosition;
                            break;
                        case ConsoleKey.X:
                            context.Order_Details.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentOrderDetailPosition);
                            currentOrderDetailPosition = (currentOrderDetailPosition >= elements.Count) ? elements.Count - 1 : currentOrderDetailPosition;
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
